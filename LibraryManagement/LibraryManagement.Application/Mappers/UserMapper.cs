using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.Mappers
{
    public static class UserMapper
    {
        public static UserToReturnDTO ToUserToReturnDTO(this User user, string accessToken)
        {
            return new UserToReturnDTO
            {
                Username = user.Username,
                Email = user.Email,
                AccessToken = accessToken
            };
        }

        public static User ToUser(this UserToRegisterDTO userToRegisterDTO, byte[] passwordHash, byte[] passwordSalt)
        {
            return new User
            {
                Username = userToRegisterDTO.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = userToRegisterDTO.Role,
                Email = userToRegisterDTO.Email
            };
        }
    }
}
