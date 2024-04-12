using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TheLegoProject.Controllers;

[Authorize(Roles = "Admin")] // Only users in the "Admin" role can access actions in this controller
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
    // GET: Admin/DeleteUser/5
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return View("Error"); // Or a custom error message if ID is not provided
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return View("Error"); // Or a custom message if user not found
        }

        return View(user); // Passing user to the view to show details and ask for confirmation
    }
// POST: Admin/ConfirmDelete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelete(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle failure: You might want to log the failure or show a message
                ModelState.AddModelError("", "Failed to delete user.");
                return View("DeleteUser", user); // Redirect back to delete confirmation if failure
            }
        }

        return View("Error"); // User not found or some other issue
    }
    [HttpGet]
    public async Task<IActionResult> EditUser()
    {
        var userId = _userManager.GetUserId(User); // Gets the current logged-in user ID
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return View("Error");
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(IdentityUser userFromForm)
    {
        var userId = _userManager.GetUserId(User);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return View("Error");
        }

        if (!ModelState.IsValid)
        {
            return View(userFromForm);
        }

        // Update the properties you want to change
        user.Email = userFromForm.Email;
        // Assume we allow the user to change their email and UserName
        user.UserName = userFromForm.Email;

        // Save the changes
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(userFromForm);
        }
    }
}
