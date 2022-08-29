﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem item)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new TodoViewModel { Items = await _todoItemService.GetIncompleteItemsAsync() });

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

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction(nameof(Index));

            try
            {
                await _todoItemService.MarkDoneAsync(id);
            }
            catch(InvalidOperationException)
            {
                return BadRequest("Could not mark the item done.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
