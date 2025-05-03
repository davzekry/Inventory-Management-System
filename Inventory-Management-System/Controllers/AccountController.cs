using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventory_Management_System.DTO.Accounts;
using Inventory_Management_System.DTO.AppUsers;
using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace Inventory_Management_System.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> manager;
        private readonly IConfiguration config;

        public AccountController(UserManager<AppUser> manager, IConfiguration config)
        {
            this.manager = manager;
            this.config = config;
        }

        [HttpPost]
        public async Task<ActionResult> Register(DTORegister userFromForm)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = userFromForm.Name,
                    PasswordHash = userFromForm.Password,
                    Email = userFromForm.Email,
                };

                IdentityResult result = await manager.CreateAsync(appUser, userFromForm.Password);
                if (result.Succeeded)
                    return Ok("Account Created Successfully!!");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
           
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<ActionResult> Login(DTOLogin userFromConsumer)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = await manager.FindByNameAsync(userFromConsumer.UserName);
                
                if(appUser != null)
                {
                    bool result = await manager.CheckPasswordAsync(appUser, userFromConsumer.Password);
                    if(result)
                    {
                        var roles = await manager.GetRolesAsync(appUser);
                        string JtiString = Guid.NewGuid().ToString();

                        List<Claim> claimList = new();
                        claimList.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id));
                        claimList.Add(new Claim(ClaimTypes.Name, appUser.UserName));
                        claimList.Add(new Claim(ClaimTypes.Email, appUser.Email));
                        claimList.Add(new Claim(JwtRegisteredClaimNames.Jti, JtiString));
                        if(roles != null)
                        {
                            foreach (var role in roles)
                                claimList.Add(new Claim(ClaimTypes.Role, role));
                        }

                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken myToken = new
                            (
                                expires: DateTime.Now.AddDays(2),
                                claims: claimList,
                                signingCredentials: signingCredentials
                            );

                        return Ok(new
                        {
                            expire = DateTime.Now.AddDays(2),
                            Token = new JwtSecurityTokenHandler().WriteToken(myToken)
                        });
                    }
                    ModelState.AddModelError("", "Invalid Account or Password");
                }
            }
            return BadRequest(ModelState);
        }



    }
}
