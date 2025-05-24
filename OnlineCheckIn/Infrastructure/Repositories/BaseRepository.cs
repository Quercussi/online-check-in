using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Infrastructure.Database;

namespace OnlineCheckIn.Infrastructure.Repositories;

public abstract class BaseRepository<T>(OnlineCheckInContext dbContext) : IBaseRepository<T>
    where T : class
{
    protected readonly DbSet<T> DbSet = dbContext.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "",
        int? offset = null,
        int? limit = null)
    {
        IQueryable<T> query = DbSet;

        if (filter != null)
            query = query.Where(filter);

        query = includeProperties
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        if (orderBy != null)
            query = orderBy(query);

        if (offset.HasValue)
            query = query.Skip(offset.Value);
        if (limit.HasValue)
            query = query.Take(limit.Value);

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    
    public virtual async Task<bool> DeleteByIdAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null) return false;

        DbSet.Remove(entity);
        return true; // TODO: implement proper deletion logic
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity; 
    }

    public virtual async Task<List<T>> AddRangeAsync(List<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
        return entities; 
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return await Task.FromResult(entity);; 
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }
}