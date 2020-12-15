using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly UserDbContext _userDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings,
           UserDbContext userDbContext, RoleManager<IdentityRole> roleManager)
        {
            _userDbContext = userDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Login loginParam)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(loginParam.Username, loginParam.Password, false, false);

                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginParam.Username);
                    var roles = await _userManager.GetRolesAsync(user);


                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("Customerid", user.Id),
                            new Claim("CustomerName", user.UserName),
                            new Claim("Email", user.Email),
                            new Claim("Roles",roles[0])

                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtSecurityToken = tokenHandler.WriteToken(token);

                    var obj = new
                    {
                        token = jwtSecurityToken,
                        UserId = user.Id,
                        UserName = user.UserName,
                        Role = roles[0]
                    };
                    return Ok(obj);
                }
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        //[Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Register(Customer model)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser()
                    {
                        UserName = model.Name,
                        Email = model.Email
                    };
                    var userResult = await _userManager.CreateAsync(user, model.Password);

                    if (userResult.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, "customer");
                        if (roleResult.Succeeded)
                        {
                            return Ok(user);
                        }
                    }
                    else
                    {
                        foreach (var error in userResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return BadRequest(ModelState.Values);
                    }
                }
                return BadRequest(ModelState.Values);
            }
            catch (Exception E)
            {
                ModelState.AddModelError("", E.Message);
                return BadRequest(ModelState.Values);
            }
        }

        [AllowAnonymous]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
