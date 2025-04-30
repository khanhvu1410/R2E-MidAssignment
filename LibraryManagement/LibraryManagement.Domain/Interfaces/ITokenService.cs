using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
