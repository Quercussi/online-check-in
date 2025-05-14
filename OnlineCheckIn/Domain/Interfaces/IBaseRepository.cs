namespace OnlineCheckIn.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteByIdAsync(string id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> AddRangeAsync(List<T> entities);
}