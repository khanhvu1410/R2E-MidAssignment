namespace LibraryManagement.Application.DTOs.Auth
{
    public class UserToLoginDTO
    {
        public string Username { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
