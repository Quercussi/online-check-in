using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Domain.Interfaces;

public interface IHotelRepository : IBaseRepository<Hotel>
{
    Task<IEnumerable<Hotel>> GetHotelsByCompanyIdAsync(Guid companyId, int? offset, int? limit, string? orderBy, bool ascending);
}