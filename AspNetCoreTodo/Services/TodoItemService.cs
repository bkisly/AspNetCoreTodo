﻿using AspNetCoreTodo.Models;
using AspNetCoreTodo.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context) => _context = context;

        public Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            return (from TodoItem item in _context.Items
                    where item.IsDone == false
                    select item).ToArrayAsync();
        }
    }
}
