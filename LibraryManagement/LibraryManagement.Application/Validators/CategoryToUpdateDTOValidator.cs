using FluentValidation;
using LibraryManagement.Application.DTOs.Category;

namespace LibraryManagement.Application.Validators
{
    public class CategoryToUpdateDTOValidator : AbstractValidator<CategoryToUpdateDTO>
    {
        public CategoryToUpdateDTOValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .WithMessage("Name cannot be whitespace.");
        }
    }
}
