using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Infrastructure.Database;

namespace OnlineCheckIn.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    protected readonly OnlineCheckInContext OnlineCheckInContext;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(OnlineCheckInContext dbContext)
    {
        OnlineCheckInContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = DbSet;

        if (filter != null)
            query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        else
            return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<bool> DeleteByIdAsync(string id)
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