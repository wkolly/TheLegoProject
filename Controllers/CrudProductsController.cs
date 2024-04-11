using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheLegoProject.Models;
using TheLegoProject.Data;

namespace TheLegoProject.Controllers
{
    public class CrudProductsController : Controller
{
    private readonly ILegoRepository _repo;

    public CrudProductsController(ILegoRepository repo)
    {
        _repo = repo;
    }

    public IActionResult DisplayProducts()
    {
        var products = _repo.GetAllProducts(); // Assuming GetAllProducts is the synchronous version
        return View(products);
    }

    public IActionResult ProductDetails(int id)
    {
        var product = _repo.GetProductById(id); // Assuming GetProductById is the synchronous version
        if (product == null)
        {
            return View("ProductNotFound");
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult CreateProduct()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _repo.AddProduct(product); // Assuming AddProduct is the synchronous version
            return RedirectToAction(nameof(DisplayProducts));
        }
        return View(product);
    }

    [HttpGet]
     public IActionResult EditProduct(int id)
     {
         var product = _repo.GetProductById(id); // Assuming GetProductById is the synchronous version
         if (product == null)
         {
             return View("ProductNotFound");
         }
         return View(product);
     }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditProduct(int id,[Bind("ProductId,Year,NumParts,Price,PrimaryColor,SecondaryColor,Description,ImgLink,Category,Subcategory")] Product product)
    {
        if (ModelState.IsValid)
        {
            _repo.UpdateProduct(id, product);
            return RedirectToAction(nameof(DisplayProducts));
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult DeleteProduct(int id)
    {
        var product = _repo.GetProductById(id); // Assuming GetProductById is the synchronous version
        if (product == null)
        {
            return View("ProductNotFound");
        }
        return View(product);
    }

    [HttpPost, ActionName("DeleteProduct")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _repo.DeleteProduct(id); // Assuming DeleteProduct is the synchronous version
        return RedirectToAction(nameof(DisplayProducts));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

}
