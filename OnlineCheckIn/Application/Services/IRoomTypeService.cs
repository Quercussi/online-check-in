using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public interface IRoomTypeService
{
    Task<RoomType?> GetRoomTypeById(Guid id);

    Task<IEnumerable<RoomType>> GetRoomTypesByHotelId(
        Guid hotelId,
        int? offset = 0,
        int? limit = 30,
        string? orderBy = null,
        bool ascending = true);
    
    Task<RoomType> AddRoomType(RoomType roomType);
}