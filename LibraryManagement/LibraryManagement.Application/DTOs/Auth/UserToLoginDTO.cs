namespace LibraryManagement.Application.DTOs.Auth
{
    public class UserToLoginDTO
    {
        public string? Username { get; set; }

        public required string Password { get; set; }
    }
}
