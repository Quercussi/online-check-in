using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;

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
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>>? orderByFunc = 
            !string.IsNullOrWhiteSpace(orderBy)
                ? q => ascending
                    ? q.OrderBy(h => EF.Property<object>(h, orderBy))
                    : q.OrderByDescending(h => EF.Property<object>(h, orderBy))
                : null;

        return await GetAsync(
            filter:    h => h.Company.Id == companyId,
            orderBy:   orderByFunc,
            includeProperties: "Company",
            offset:    offset,
            limit:     limit
        );
    }
}