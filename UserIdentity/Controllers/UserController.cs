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
                UserName = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result
            
        });

        return View(renderedUsers);
    }
    [HttpGet]
    public async Task<IActionResult> AddRoleToUser([FromQuery]string userId) {

        var user = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(user);
        var roles = await _roleManager.Roles.ToListAsync();

        var filteredRoles = roles.Where(r=>!userRoles.Contains(r.Name)).Select(r=>new RoleFormViewModel(){
            Name = r.Name,
            IsSelected = false
        }).ToList();

        var model = new RoleUserFormViewModel(){Roles = filteredRoles, UserId = userId};
        
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddRoleToUser(RoleUserFormViewModel model) {

        var user = await _userManager.FindByIdAsync(model.UserId.ToString());
        var selectedRoles = model.Roles.Where(r=>r.IsSelected).Select(r=>r.Name);

        await _userManager.AddToRolesAsync(user, selectedRoles);

        return RedirectToAction(nameof(Index));
    }

    
    [HttpGet]
    public async Task<IActionResult> DeleteRoleFromUser([FromQuery]string userId) {

        var user = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(user);

        var model = new RoleUserFormViewModel(){Roles = userRoles.Select(r=>new RoleFormViewModel(){
            Name = r,
            IsSelected = false
        }).ToList(), UserId = userId};
        
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteRoleFromUser(RoleUserFormViewModel model) {
        
        var user = await _userManager.FindByIdAsync(model.UserId);
        await _userManager.RemoveFromRolesAsync(user, model.Roles.Where(r=>r.IsSelected).Select(r=>r.Name));
        
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> AddUser() {
        var roles = await _roleManager.Roles.Select(r=>new RoleFormViewModel(){IsSelected = false,Name = r.Name}).ToListAsync();
        var newUser = new AddUserFormViewModel() {
            Roles = roles
        };
        return View(newUser);
    }
    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserFormViewModel model) {
        
        if (!ModelState.IsValid) {
            return View(model);
        }
        else if (model.Roles.Where(r=>r.IsSelected).Count() == 0)    {
            ModelState.AddModelError("Roles", "You must select at least one role");
            return View(model);
        }
        else if (await _userManager.FindByNameAsync(model.UserName) != null) {
            ModelState.AddModelError("UserName", $"Username '{model.UserName}' is already taken. ");
            return View(model);
        }
        else if (await _userManager.FindByEmailAsync(model.Email) != null) {
            ModelState.AddModelError("Email", "The email exists already");
            return View(model);
        }

        var user = new ApplicationUser() {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.UserName
        };


        var result = await _userManager.CreateAsync(user,model.Password);

        if (result.Succeeded) {
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r=>r.IsSelected).Select(r=>r.Name).ToList());
        }
        else {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Roles", error.Description);
            }
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }
    // let's make views to manage the admin to edit other users , 
    public async Task<IActionResult> EditUser(string userId) {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is not ApplicationUser) {
            return NotFound();
        }
        
        var userData = new EditUserFormViewModel() {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName,
        };

        return View(userData);
    }
    [HttpPost]
    // [ValidateAntiForgeryToken] look how to use it , 
    public async Task<IActionResult> EditUser(EditUserFormViewModel model) {
        if (!ModelState.IsValid) {
            return View(model);
        }
        // check about the email 

        var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

        if (userWithSameEmail != null && userWithSameEmail.Id  != model.UserId) {
            ModelState.AddModelError("Email", $"The email {model.Email} already exists ");
            return View(model);
        }

        // check about the username 
        var userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);

        if (userWithSameUserName != null && userWithSameUserName.Id  != model.UserId) {
            ModelState.AddModelError("UserName", $"The username {model.UserName} already exists ");
            return View(model);
        }

        userWithSameEmail.FirstName = model.FirstName;
        userWithSameEmail.LastName = model.LastName;
        userWithSameEmail.Email = model.Email;
        userWithSameEmail.UserName =model.UserName;
        await _userManager.UpdateAsync(userWithSameEmail);

        return Redirect(nameof(Index));
    }
}