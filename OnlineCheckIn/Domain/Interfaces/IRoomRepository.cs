using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Domain.Interfaces;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(Guid roomTypeId, int? offset, int? limit, string? orderBy, bool ascending);
    Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(Guid hotelId, int? offset, int? limit, string? orderBy, bool ascending);
}