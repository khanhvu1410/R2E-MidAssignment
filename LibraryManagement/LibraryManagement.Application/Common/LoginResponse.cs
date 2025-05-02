using LibraryManagement.Application.DTOs.Auth;

namespace LibraryManagement.Application.Common
{
    public class LoginResponse
    {
        public UserToReturnDTO? User {  get; set; }

        public string? AccessToken { get; set; }
    }
}
