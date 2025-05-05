using System.Linq.Expressions;
using LibraryManagement.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<PagedResult<T>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter, params Expression<Func<T, object?>>[] includes);

        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids);

        Task<IEnumerable<T>> GetFiltersAsync(params Expression<Func<T, bool>>[] filters);

        Task<T?> GetAsync(Expression<Func<T, bool>> expression);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
