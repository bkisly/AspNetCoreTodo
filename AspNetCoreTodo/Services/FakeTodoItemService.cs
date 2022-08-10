using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Services
{
    public class FakeTodoItemService : ITodoItemService
    {
        public Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            TodoItem item1 = new() { Title = "Learn ASP.NET Core", DueAt = DateTime.Now.AddDays(1) };
            TodoItem item2 = new() { Title = "Build awesome apps", DueAt = DateTime.Now.AddDays(2) };
            return Task.FromResult(new[] { item1, item2 });
        }
    }
}
