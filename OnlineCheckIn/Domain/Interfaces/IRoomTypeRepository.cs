using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Domain.Interfaces;

public interface IRoomTypeRepository : IBaseRepository<RoomType>
{
    Task<IEnumerable<RoomType>> GetRoomTypesByHotelIdAsync(Guid hotelId, int? offset, int? limit, string? orderBy, bool ascending);
}