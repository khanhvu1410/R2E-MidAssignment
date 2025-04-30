using FluentValidation;
using LibraryManagement.Application.DTOs.Auth;

namespace LibraryManagement.Application.Validators
{
    public class UserToRegisterDTOValidator : AbstractValidator<UserToRegisterDTO>
    {
        public UserToRegisterDTOValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
