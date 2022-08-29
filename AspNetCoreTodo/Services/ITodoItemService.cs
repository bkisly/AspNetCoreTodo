using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        public Task<TodoItem[]> GetIncompleteItemsAsync();
        public Task AddItemAsync(TodoItem item);
        public Task MarkDoneAsync(Guid id);
    }
}
