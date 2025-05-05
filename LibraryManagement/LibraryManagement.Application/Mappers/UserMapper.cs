using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    public static class UserMapper
    {
        public static UserToReturnDTO ToUserToReturnDTO(this User user)
        {
            return new UserToReturnDTO
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
            };
        }

        public static User ToUser(this UserToRegisterDTO userToRegisterDTO, byte[]? passwordHash, byte[]? passwordSalt)
        {
            return new User
            {
                Username = userToRegisterDTO.Username,
                PasswordHash = passwordHash ?? default!,
                PasswordSalt = passwordSalt ?? default!,
                Role = userToRegisterDTO.Role,
                Email = userToRegisterDTO.Email
            };
        }
    }
}
