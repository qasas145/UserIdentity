using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UserIdentity.Controllers.Api
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly UserManager<ApplicationUser> _userManager;


        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return NotFound();
            var result = await  _userManager.DeleteAsync(user);
            if (!result.Succeeded) 
                throw new Exception("The user hasn't deleted");
            return Ok();
        
        }
        }
}