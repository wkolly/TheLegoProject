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
    
    [Authorize(Roles="Admin")]
    public IActionResult Products(int pageNum, int? pageSize)
    {
        pageSize ??= 5; // Default to 5 if no value is provided
        var pageSizeOptions = new List<int> { 5, 10, 15 }; // The available page size options

        var blah = new ProductsListViewModel
        {
            Products = _repo.Products
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * pageSize.Value)
                .Take(pageSize.Value),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize.Value,
                TotalItems = _repo.Products.Count()
            },

            PageSizeOptions = pageSizeOptions
            

        };
        
        return View(blah);
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
