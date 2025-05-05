using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = default!;

        public byte[] PasswordHash { get; set; } = default!;
        
        public byte[] PasswordSalt { get; set; } = default!;

        public UserRole Role { get; set; } = UserRole.NormalUser;

        public string? Email { get; set; }

        public ICollection<BookBorrowingRequest>? BookBorrowingRequests { get; set; }
    }
}
