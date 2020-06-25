
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace weddingplanner.Models
{
    public class PasswordValidationsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
        var reg = new Regex(@"^(?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[!@#$%^&*+=])[A-Za-z\d,!@#$%^&*+=]{8,}$");
            if (value == null)
                return new ValidationResult("Password is required");
            if (!reg.IsMatch((string)value))
                return new ValidationResult("Password must be 8 characters long, contain at least 1 special character, 1 number, 1 lowercase character, and 1 uppercase character");
            return ValidationResult.Success;
        }
    }
}