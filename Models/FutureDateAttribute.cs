  
using System.ComponentModel.DataAnnotations;
using System;

namespace weddingplanner.Models
{
  public class FuturedateAttribute : ValidationAttribute
  {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
          if ((DateTime) value < DateTime.Today)
          {
              return new ValidationResult("Wedding date must be in the future.");
          }
          return ValidationResult.Success;
      }
  }
}