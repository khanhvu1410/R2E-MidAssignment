using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<IPagedResult<T>> GetPagedAsync(int pageIndex, int pageSize);

        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task<IDbContextTransaction> BeginTransactionAsync();

        IQueryable<T> GetQueryable();
    }
}
