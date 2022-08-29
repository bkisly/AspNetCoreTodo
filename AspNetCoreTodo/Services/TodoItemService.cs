using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context) => _context = context;

        public async Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user)
        {
            return await (from TodoItem item in _context.Items
                    where item.IsDone == false && item.Userid == user.Id
                    select item).ToArrayAsync();
        }

        public async Task AddItemAsync(TodoItem item, IdentityUser user)
        {
            if (item.DueAt == null) item.DueAt = DateTime.Now.AddDays(3);
            item.Userid = user.Id;
            await _context.AddAsync(item);
            _context.SaveChanges();
        }

        public async Task MarkDoneAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items.SingleAsync(i => i.Id == id && i.Userid == user.Id);
            item.IsDone = true;
            _context.SaveChanges();
        }
    }
}
