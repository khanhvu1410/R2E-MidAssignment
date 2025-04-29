using FluentValidation;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Validators
{
    public class CategoryToAddDTOValidator : AbstractValidator<CategoryToAddDTO>
    {
        public CategoryToAddDTOValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .WithMessage("Name cannot be whitespace.");
        }
    }
}
