using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _todoItemService.GetIncompleteItemsAsync();
            TodoViewModel viewModel = new() { Items =  items };
            return View(viewModel);
        }

        public async Task<IActionResult> AddItem(TodoItem item)
        {
            if(!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            try
            {
                await _todoItemService.AddItemAsync(item);
            }
            catch(InvalidOperationException)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
