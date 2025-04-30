using LibraryManagement.Application.DTOs.Auth;

namespace LibraryManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserToRegisterDTO userToRegisterDTO);

        Task<UserToReturnDTO> LoginAsync(UserToLoginDTO userToLoginDTO);
    }
}
