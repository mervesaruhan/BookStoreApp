using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO
{
    public class RoleValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not UserRole role || !Enum.IsDefined(typeof(UserRole), role))
            {
                return new ValidationResult("Geçersiz kullanıcı rolü! Sadece Admin (0) veya Customer (1) değerlerini kabul eder.");
            }

            return ValidationResult.Success;
        }
    }
}
