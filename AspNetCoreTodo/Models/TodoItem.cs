using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Userid { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public string Title { get; set; } = null!;
        [Display(Name = "Due at"), DateLaterThanNow] public DateTimeOffset? DueAt { get; set; }
    }
}
