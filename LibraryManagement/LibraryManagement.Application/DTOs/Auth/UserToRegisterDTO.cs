using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.Auth
{
    public class UserToRegisterDTO
    {
        public string Username { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public UserRole Role { get; set; } = UserRole.NormalUser;
    }
}
