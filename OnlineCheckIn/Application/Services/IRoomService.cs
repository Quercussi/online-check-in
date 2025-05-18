using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public interface IRoomService
{
    Task<Room?> GetRoomById(Guid id);

    Task<IEnumerable<Room>> GetRoomsByRoomTypeId(
        Guid roomTypeId,
        int? offset = 0,
        int? limit = 30,
        string? orderBy = null,
        bool ascending = true);
    
    Task<IEnumerable<Room>> GetRoomsByHotelId(
        Guid hotelId,
        int? offset = 0,
        int? limit = 30,
        string? orderBy = null,
        bool ascending = true);
    
    Task<Room> AddRoom(Room room);
}