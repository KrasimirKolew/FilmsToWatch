using FilmsToWatch.Models.UserModels;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Areas.SuperAdmin.Controllers
{
    public class HomeController : SuperAdminBaseController
    {
        private readonly IUserService userService;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<IdentityUser> userManager;

        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(
            UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            IUserService _userService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            userService = _userService;
        }

        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            var model = new AddRoleViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {

            if (await roleManager.RoleExistsAsync(model.RoleName) == false)
            {
                await roleManager.CreateAsync(userService.CreateRole(model.RoleName));
            };

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GiveRole()
        {
            var model = new UserToRoleViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GiveRole(UserToRoleViewModel model)
        {

            if (await roleManager.RoleExistsAsync(model.RoleName))
            {
                var user = await userManager.FindByNameAsync(model.UserName);



                if (await userManager.IsInRoleAsync(user, model.RoleName) == false
                    && user != null)
                {
                    await userManager.AddToRoleAsync(user, model.RoleName);
                }
            };

            return View();
        }
    }
}
