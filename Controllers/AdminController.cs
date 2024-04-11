using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TheLegoProject.Controllers;

[Authorize] // Adjust according to your authorization strategy
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var usersWithRoles = new List<(string UserId, string Email, IList<string> Roles)>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            usersWithRoles.Add((user.Id, user.Email, roles));
        }

        ViewBag.UsersWithRoles = usersWithRoles;
        ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            // Handle the case where the user was not found
            return View("Error");
        }

        // Check if the user already has the role
        var userHasRole = await _userManager.IsInRoleAsync(user, roleName);
        if (userHasRole)
        {
            // User already has this role, no need to do anything
            return RedirectToAction(nameof(Index));
        }

        // Remove all roles currently assigned to the user
        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            // Handle the case where the role removal failed
            // You can log the error or add error details to ModelState or ViewBag to display in the view
        }

        // Add the user to the new role
        var addResult = await _userManager.AddToRoleAsync(user, roleName);
        if (addResult.Succeeded)
        {
            // If the user was successfully added to the role, redirect back to the index
            return RedirectToAction(nameof(Index));
        }
        else
        {
            // Handle the case where the role addition failed
            // You can log the error or add error details to ModelState or ViewBag to display in the view
        }

        // If we're here, something went wrong; redirect back to the index view
        return RedirectToAction(nameof(Index));
    }

    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(string email, string password)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // User creation was successful
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Something failed, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        // Model state is invalid or user creation failed
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Handle success
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle failure
            }
        }
        // User not found or some other issue
        return RedirectToAction(nameof(Index));
    }

}
