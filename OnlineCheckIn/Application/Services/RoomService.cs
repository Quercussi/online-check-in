using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public class RoomService(IUnitOfWork unitOfWork): IRoomService
{
    public async Task<Room?> GetRoomById(Guid id)
    {
        return await unitOfWork.RoomRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Room>> GetRoomsByRoomTypeId(
        Guid roomTypeId, 
        int? offset = 0, 
        int? limit = 30, 
        string? orderBy = null, 
        bool ascending = true)
    {
        return await unitOfWork.RoomRepository.GetRoomsByRoomTypeIdAsync(
            roomTypeId, 
            offset, 
            limit, 
            orderBy, 
            ascending);
    }

    public async Task<IEnumerable<Room>> GetRoomsByHotelId(
        Guid hotelId, 
        int? offset = 0, 
        int? limit = 30, 
        string? orderBy = null, 
        bool ascending = true)
    {
        return await unitOfWork.RoomRepository.GetRoomsByHotelIdAsync(
            hotelId, 
            offset, 
            limit, 
            orderBy, 
            ascending);
    }

    public async Task<Room> AddRoom(Room room)
    {
        Room result = await unitOfWork.RoomRepository.AddAsync(room);
        await unitOfWork.SaveAsync();
        return result;
    }
}