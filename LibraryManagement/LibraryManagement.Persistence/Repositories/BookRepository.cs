using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class BookRepository : GenericRepository<Book>, IGenericRepository<Book>
    {
        public BookRepository(LibraryManagementDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<Book>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Books
                .Where(b => ids.Contains(b.Id))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
