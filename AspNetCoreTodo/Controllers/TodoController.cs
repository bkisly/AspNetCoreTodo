using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.ViewModels;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var items = await _todoItemService.GetIncompleteItemsAsync(user);
            TodoViewModel viewModel = new() { Items =  items };
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem item)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
                return View(nameof(Index), new TodoViewModel { Items = await _todoItemService.GetIncompleteItemsAsync(user) });

            try
            {
                await _todoItemService.AddItemAsync(item, user);
            }
            catch(InvalidOperationException)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (id == Guid.Empty) return RedirectToAction(nameof(Index));

            try
            {
                await _todoItemService.MarkDoneAsync(id, user);
            }
            catch(InvalidOperationException)
            {
                return BadRequest("Could not mark the item done.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
