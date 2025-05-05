using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.Auth
{
    public class UserToReturnDTO
    {
        public string Username { get; set; } = default!;

        public string? Email { get; set; }

        public UserRole Role { get; set; }
    }
}
