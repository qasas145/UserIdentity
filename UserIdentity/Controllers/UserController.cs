using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UserController : Controller {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public UserController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index() {

        var users = await _userManager.Users.ToListAsync();   
        var renderedUsers = users.Select(user=>new UserViewModel() {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result
            
        });

        return View(renderedUsers);
    }
    [HttpGet]
    public async Task<IActionResult> AddRoleToUser([FromQuery]string userId) {

        var user = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(user);

        var roles = await _roleManager.Roles.ToListAsync();
        var filteredRoles = roles.Where(r=>!userRoles.Contains(r.Name)).Select(r=>r.Name);

        var model = new RoleUserFormModel(){Roles = filteredRoles, UserId = userId};
        
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddRoleToUser(RoleUserFormModel RoleUserFormModel) {

        var user = await _userManager.FindByIdAsync(RoleUserFormModel.UserId.ToString());

        await _userManager.AddToRoleAsync(user, RoleUserFormModel.Role);

        return RedirectToAction(nameof(Index));
    }

    
    [HttpGet]
    public async Task<IActionResult> DeleteRoleFromUser([FromQuery]string userId) {

        var user = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(user);

        var model = new RoleUserFormModel(){Roles = userRoles, UserId = userId};
        
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteRoleFromUser(RoleUserFormModel RoleUserFormModel) {
        
        var user = await _userManager.FindByIdAsync(RoleUserFormModel.UserId);
        await _userManager.RemoveFromRoleAsync(user, RoleUserFormModel.Role);
        
        return RedirectToAction(nameof(Index));
    }
}