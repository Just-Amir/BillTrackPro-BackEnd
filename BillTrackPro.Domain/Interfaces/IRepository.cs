using System.Linq.Expressions;
using BillTrackPro.Domain.Common;

namespace BillTrackPro.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<BillTrackPro.Domain.Common.PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, string? orderBy = null, bool isDescending = false, params Expression<Func<T, object>>[] includes);
}
