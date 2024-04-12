using System.Diagnostics;
using System.Security.Claims;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using TheLegoProject.Models;
using Microsoft.AspNetCore.Authorization;
using TheLegoProject.Models.ViewModels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using TheLegoProject.Models;
using TheLegoProject.Models.ViewModels;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Operations;
using TheLegoProject.Infrastructure;

namespace TheLegoProject.Controllers;

public class HomeController : Controller
{
    
    private readonly ILegoRepository _repo;
        
    public HomeController(ILegoRepository temp)
    {
        _repo = temp;
    }
    
    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            // Hardcoded customer ID for demonstration purposes
            int customerId = 1; // Replace with the actual logged-in customer ID

            // Fetch personalized recommendations for the logged-in user and evaluate them in-memory
            var customerRecommendations = _repo.CustRecommendations
                .Where(c => c.CustomerId == customerId)
                .ToList(); // Bring into memory to use client-side evaluation for the next operations

            var productNames = customerRecommendations
                .SelectMany(c => new[] { c.Rec1, c.Rec2, c.Rec3, c.Rec4, c.Rec5 }) // Now we're in-memory, so SelectMany will work
                .Distinct();

            var personalizedRecData = _repo.Products
                .Where(p => productNames.Contains(p.Name)) // This will be translated to SQL WHERE IN
                .Select(product => new ProductRecommendationViewModel
                {
                    Name = product.Name,
                    ImgLink = product.ImgLink,
                    Price = product.Price,
                    // Map other properties as needed
                })
                .ToList();


            return View(personalizedRecData);

        }
        else
        {
            var recData = _repo.Recommendations
                .Join(_repo.Products, 
                    recommendation => recommendation.ProductId, // Assuming a common key exists
                    product => product.ProductId, // Replace with the actual common key
                    (recommendation, product) => new ProductRecommendationViewModel
                    {
                        Name = product.Name,
                        ImgLink = product.ImgLink,
                        Price = product.Price,
                        PopScore = recommendation.PopScore,
                        Rec1 = recommendation.Rec1,
                        Rec2 = recommendation.Rec2,
                        Rec3 = recommendation.Rec3,
                        Rec4 = recommendation.Rec4,
                        Rec5 = recommendation.Rec5
                    })
                .OrderBy(x => x.PopScore)
                .Take(6)
                .ToList();

            return View(recData);
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(ProductRecommendationViewModel model)
    {
        Console.WriteLine(model.Description);
        if (!ModelState.IsValid)
        {
            return View(model); // Return the view with validation errors
        }

        // Assuming Amount is not a collection and doesn't need to be summed up,
        // this line assumes model.Amount correctly holds the total to be charged
        // If model.Amount does not exist and you need to calculate it based on items,
        // you would need a different approach possibly involving a list of items in the model.
    
        var order = new Order
        {
            // TransactionId should be auto-generated if it's an identity column in the database,
            CustomerId = model.CustomerId,
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            DayOfWeek = DateTime.UtcNow.DayOfWeek.ToString(),
            Time = DateTime.UtcNow.Hour,
            EntryMode = "Online",  // This should match your business logic or data model requirements
            Amount = model.Amount, // This should be a decimal or double type in your Order model
            TypeOfTransaction = "Sale",
            CountryOfTransaction = "USA",
            ShippingAddress = model.ShippingAddress,
            Bank = "DefaultBank",
            TypeOfCard = "Visa",
            Fraud = 0 // Assuming default to no fraud detected
        };

        _repo.AddOrder(order);
        await _repo.SaveChangesAsync();

        // Assuming you want to redirect to an action that shows order details or confirmation
        // Redirect to a confirmation page or a similar one after the order is saved
        return View(model);
    }



    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
       
    }
    
    public IActionResult Products(int pageNum = 1, int? pageSize = null, string selectedCategory = "", string selectedColor = "")
    {
        Console.WriteLine(pageSize);
        
        pageSize ??= 5;
        Console.WriteLine(pageSize);// Default to 5 if no value is provided
        var pageSizeOptions = new List<int> { 5, 10, 15 }; // The available page size options
        
        // Filter based on category and color
        var filteredProducts = _repo.Products.AsQueryable();

        if (!string.IsNullOrEmpty(selectedCategory))
        {
            filteredProducts = filteredProducts.Where(p => p.Category == selectedCategory);
        }

        if (!string.IsNullOrEmpty(selectedColor))
        {
            filteredProducts = filteredProducts.Where(p => p.PrimaryColor == selectedColor);
        }

        // Get distinct categories and colors for the dropdowns
        var categories = _repo.Products.Select(p => p.Category).Distinct().ToList();
        var colors = _repo.Products.Select(p => p.PrimaryColor).Distinct().ToList();

        // Apply pagination after filtering
        var paginatedProducts = filteredProducts
            .OrderBy(x => x.Name)
            .Skip((pageNum - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToList(); // Convert to list here, assuming the repository call doesn't execute immediately

        var viewModel = new ProductsListViewModel
        {
            Products = paginatedProducts,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize.Value,
                TotalItems = filteredProducts.Count() // Count the filtered list
            },
            PageSizeOptions = pageSizeOptions,
            SelectedCategory = selectedCategory,
            SelectedColor = selectedColor,
            Categories = categories,
            Colors = colors
        };

        return View(viewModel);
    }
    
    public IActionResult ProductDetails(string id)
    {
    var productRecommendation = _repo.Products
        .Join(
            _repo.Recommendations,
            product => product.ProductId,
            recommendation => recommendation.ProductId,
            (product, recommendation) => new ProductRecommendationViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                ImgLink = product.ImgLink,
                Price = product.Price,
                PopScore = recommendation.PopScore,
                Rec1 = recommendation.Rec1,
                Rec2 = recommendation.Rec2,
                Rec3 = recommendation.Rec3,
                Rec4 = recommendation.Rec4,
                Rec5 = recommendation.Rec5,
                PrimaryColor = product.PrimaryColor,
                SecondaryColor = product.SecondaryColor,
                Description = product.Description,
                Category = product.Category,
                Subcategory = product.Subcategory,
                Year = product.Year,
                NumParts = product.NumParts,
                Rec1ImgLink = GetImageLink(recommendation.Rec1),
                Rec2ImgLink = GetImageLink(recommendation.Rec2),
                Rec3ImgLink = GetImageLink(recommendation.Rec3),
                Rec4ImgLink = GetImageLink(recommendation.Rec4),
                Rec5ImgLink = GetImageLink(recommendation.Rec5)
            }
        )
        .FirstOrDefault(pr => pr.Name == id);

    if (productRecommendation == null)
    {
        return NotFound();
    }

    return View("ProductDetails", productRecommendation);
}

    private static string GetImageLink(string productName)
    {
        if (string.IsNullOrEmpty(productName))
        {
            return "/images/default.jpg";
        }

        using (var context = new LegoDatabase2Context())
        {
            var product = context.Products.FirstOrDefault(p => p.Name == productName);

            if (product != null && !string.IsNullOrEmpty(product.ImgLink))
            {
                return product.ImgLink;
            }
        }

        return "/images/default.jpg";
    }

    public IActionResult Checkout(string id)
    {
        // Check if the user is logged in
        if (!User.Identity.IsAuthenticated)
        {
            // If the user is not logged in, redirect them to the home page
            return RedirectToAction("Index", "Home");
        }

        // If the user is logged in, display the Checkout view
        return View();
    }


    private int GenerateRandomCustomerId()
    {
        // Generate a random customer ID
        var random = new Random();
        return random.Next(1, 1000); // Adjust the range as per your requirements
    }
    
    public IActionResult Bag()
    {
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
