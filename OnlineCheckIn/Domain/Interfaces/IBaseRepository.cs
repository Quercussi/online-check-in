using System.Linq.Expressions;

namespace OnlineCheckIn.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string includeProperties,
        int? offset = null,
        int? limit = null);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> AddRangeAsync(List<T> entities);
}