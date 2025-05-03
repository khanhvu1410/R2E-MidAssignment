using FluentValidation;
using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.Validators
{
    public class BookToAddDTOValidator : AbstractValidator<BookToAddDTO>
    {
        public BookToAddDTOValidator() 
        {
            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 character")
                .WithMessage("Title cannot be whitespace.");

            RuleFor(b => b.Author)
                .NotEmpty().WithMessage("Author name is required.")
                .MaximumLength(100).WithMessage("Author name cannot exceed 100 characters");

            RuleFor(b => b.ISBN)
                .Length(10, 13).WithMessage("ISBN must be 10 or 13 characters")
                .Matches(@"^[0-9]+$").WithMessage("ISBN contains invalid characters");

            RuleFor(b => b.PublicationYear)
                .NotNull().WithMessage("Publication year is required.")
                .InclusiveBetween(1800, DateTime.UtcNow.Year)
                .WithMessage($"Publication year must be between 1800 and {DateTime.UtcNow.Year}.");

            RuleFor(b => b.Quantity)
                .NotEmpty().WithMessage("Quantity is required.");

            RuleFor(b => b.CategoryId)
                .NotNull().WithMessage("Category ID is required.")
                .GreaterThan(0).WithMessage("Invalid category selection.");
        }
    }
}
