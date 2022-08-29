using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models
{
    public class DateLaterThanNowAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not null)
            {
                var dateTime = (DateTimeOffset?)value;
                return dateTime.Value > DateTime.Now ? ValidationResult.Success : new ValidationResult("Due at date must be later than today.");
            }

            return ValidationResult.Success;
        }
    }
}
