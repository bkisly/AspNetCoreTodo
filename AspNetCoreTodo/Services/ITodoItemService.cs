using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        public Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user);
        public Task AddItemAsync(TodoItem item, IdentityUser user);
        public Task MarkDoneAsync(Guid id, IdentityUser user);
    }
}
