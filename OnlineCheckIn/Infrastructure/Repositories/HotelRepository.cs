using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Utils;

namespace OnlineCheckIn.Infrastructure.Repositories;

public class HotelRepository(OnlineCheckInContext onlineCheckInContext) 
    : BaseRepository<Hotel>(onlineCheckInContext), IHotelRepository
{
    public async Task<IEnumerable<Hotel>> GetHotelsByCompanyIdAsync(
        Guid companyId, 
        int? offset, 
        int? limit,
        string? orderBy,
        bool ascending)
    {
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>>? orderByFunc = QueryUtils.CreateOrderByFunc<Hotel>(orderBy, ascending);

        return await GetAsync(
            filter:    h => h.CompanyId == companyId,
            orderBy:   orderByFunc,
            offset:    offset,
            limit:     limit
        );
    }
}