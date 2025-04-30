using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.Auth
{
    public class UserToRegisterDTO
    {
        public string? Username { get; set; }

        public string? Email { get; set; }

        public required string Password { get; set; }

        public UserRole Role { get; set; } = UserRole.NormalUser;
    }
}
