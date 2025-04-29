using FluentValidation;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Validators
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .WithMessage("Name cannot be whitespace.");
        }
    }
}
