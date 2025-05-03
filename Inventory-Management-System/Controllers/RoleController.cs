using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> manager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> manager)
        {
            this.roleManager = roleManager;
            this.manager = manager;
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(string roleName)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = roleName;
                result = await roleManager.CreateAsync(role);
                if(result.Succeeded)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> AssignAdmin(string userName)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await manager.FindByNameAsync(userName);
                if (appUser != null)
                {
                    IdentityResult result = await manager.AddToRoleAsync(appUser, "Admin");
                    if (result.Succeeded) 
                        return Ok(result);
                    return BadRequest(result);
                }
            }
            return BadRequest();
        }

        //public async void createrole(string roleName)
        //{
        //    IdentityRole role = new IdentityRole();
        //    role.Name = roleName;
        //    await roleManager.CreateAsync(role);
        //}
    }
}
