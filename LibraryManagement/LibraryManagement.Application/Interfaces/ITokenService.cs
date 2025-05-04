using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
