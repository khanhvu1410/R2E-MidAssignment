using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public required byte[] PasswordHash { get; set; }
        
        public required byte[] PasswordSalt { get; set; }

        public UserRole Role { get; set; }

        public string? Email { get; set; }

        public ICollection<BookBorrowingRequest>? BookBorrowingRequests { get; set; }
    }
}
