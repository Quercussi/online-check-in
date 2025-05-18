using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Utils;

namespace OnlineCheckIn.Infrastructure.Repositories;

public class RoomRepository(OnlineCheckInContext onlineCheckInContext)
    : BaseRepository<Room>(onlineCheckInContext), IRoomRepository
{
    public async Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(Guid roomTypeId, int? offset, int? limit, string? orderBy, bool ascending)
    {
        Func<IQueryable<Room>, IOrderedQueryable<Room>>? orderByFunc = QueryUtils.CreateOrderByFunc<Room>(orderBy, ascending);

        return await GetAsync(
            filter: r => r.RoomTypeId == roomTypeId,
            orderBy: orderByFunc,
            offset: offset,
            limit: limit
        );
    }

    public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(Guid hotelId, int? offset, int? limit, string? orderBy, bool ascending)
    {
        Func<IQueryable<Room>, IOrderedQueryable<Room>>? orderByFunc = QueryUtils.CreateOrderByFunc<Room>(orderBy, ascending);

        return await GetAsync(
            filter: r => r.RoomType.HotelId == hotelId,
            orderBy: orderByFunc,
            offset: offset,
            limit: limit
        );
    }
}