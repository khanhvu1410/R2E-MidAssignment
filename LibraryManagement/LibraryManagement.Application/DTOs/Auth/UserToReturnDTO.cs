using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.Auth
{
    public class UserToReturnDTO
    {
        public string? Username { get; set; }

        public string? Email { get; set; }

        public UserRole Role { get; set; }
    }
}
