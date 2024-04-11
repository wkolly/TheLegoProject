using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class AddRolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public AddRolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IActionResult Users()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }

    [HttpGet]

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create(IdentityRole model)
    {
        if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult(); 
        }

        return RedirectToAction("Users");
    }
    
    
    
}

