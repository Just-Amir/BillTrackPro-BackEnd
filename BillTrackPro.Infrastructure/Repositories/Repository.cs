using System.Linq.Expressions;
using BillTrackPro.Domain.Common;
using BillTrackPro.Domain.Interfaces;
using BillTrackPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using System.Linq.Dynamic.Core;
namespace BillTrackPro.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly BillTrackDbContext _context;

    public Repository(BillTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BillTrackPro.Domain.Common.PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, string? orderBy = null, bool isDescending = false, params Expression<Func<T, object>>[] includes)
    {
        var query = _context.Set<T>().AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (!string.IsNullOrWhiteSpace(orderBy))
        {
             // Simple validation to prevent injection if not handled by Dynamic Linq (it handles safely mostly)
             try 
             {
                query = query.OrderBy($"{orderBy} {(isDescending ? "desc" : "asc")}");
             }
             catch
             {
                // Fallback or ignore invalid sort
             }
        }

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new BillTrackPro.Domain.Common.PagedResult<T>(items, totalCount, page, pageSize);
    }
}
