using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheLegoProject.Models;
using Microsoft.AspNetCore.Authorization;
using TheLegoProject.Models.ViewModels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using TheLegoProject.Models;
using TheLegoProject.Models.ViewModels;

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
        var recData = _repo.Products
            .OrderBy(x => x.Name)
            .Take(6);
        return View(recData);
        
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
        pageSize ??= 5; // Default to 5 if no value is provided
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
    


    public IActionResult ProductDetails(int id)
    {
        var product = _repo.Products.FirstOrDefault(p => p.ProductId == id);
        return View("ProductDetails", product);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
