using FluentValidation;
using LibraryManagement.Application.DTOs.Auth;

namespace LibraryManagement.Application.Validators
{
    public class UserToLoginDTOValidator : AbstractValidator<UserToLoginDTO>
    {
        public UserToLoginDTOValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
