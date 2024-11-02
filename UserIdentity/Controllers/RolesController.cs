using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

public class RolesController : Controller {
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index() {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRole(RoleFormModel roleFormModel) {

        if (!ModelState.IsValid) {
            return View("Index",await _roleManager.Roles.ToListAsync());
        }

        var roleExist = await _roleManager.RoleExistsAsync(roleFormModel.Name);

        if (roleExist){

            ModelState.AddModelError("Name" , "The role exists already");
            return View("Index", await _roleManager.Roles.ToListAsync());
        }
        else {

            await _roleManager.CreateAsync(new IdentityRole(roleFormModel.Name.Trim()));
            return RedirectToAction(nameof(Index));
        }

    }
}