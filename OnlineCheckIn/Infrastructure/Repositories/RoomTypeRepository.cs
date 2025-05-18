using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Utils;

namespace OnlineCheckIn.Infrastructure.Repositories;

public class RoomTypeRepository(OnlineCheckInContext onlineCheckInContext)
    : BaseRepository<RoomType>(onlineCheckInContext), IRoomTypeRepository
{
    public async Task<IEnumerable<RoomType>> GetRoomTypesByHotelIdAsync(Guid hotelId, int? offset, int? limit, string? orderBy, bool ascending)
    {
        Func<IQueryable<RoomType>, IOrderedQueryable<RoomType>>? orderByFunc = QueryUtils.CreateOrderByFunc<RoomType>(orderBy, ascending);

        return await GetAsync(
            filter: rt => rt.HotelId == hotelId,
            orderBy: orderByFunc,
            offset: offset,
            limit: limit
        );
    }
}