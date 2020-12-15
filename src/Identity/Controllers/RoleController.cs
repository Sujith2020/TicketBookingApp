using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Identity.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("{role}")]
        public async Task<IActionResult> Create(string role)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = role
            };
            IdentityResult result = await roleManager.CreateAsync(identityRole);
            return Ok(result);
        }
    }
}
