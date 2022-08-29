using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.ViewModels
{
    public class TodoViewModel
    {
        public TodoItem[] Items { get; set; } = null!;
    }
}
