using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.ViewModels;

namespace AspNetCoreTodo.Controllers
{
    [Authorize(Roles = RoleName.Administrator)]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ManageUsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = (await _userManager.GetUsersInRoleAsync(RoleName.Administrator)).ToArray();
            var everyone = _userManager.Users.ToArray();

            ManageUsersViewModel viewModel = new() { Admins = admins, Everyone = everyone };
            return View(viewModel);
        }
    }
}
