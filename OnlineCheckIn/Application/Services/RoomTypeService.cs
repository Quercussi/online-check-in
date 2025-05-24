using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public class RoomTypeService(IUnitOfWork unitOfWork): IRoomTypeService
{
    public async Task<RoomType?> GetRoomTypeById(Guid id)
    {
        return await unitOfWork.RoomTypeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<RoomType>> GetRoomTypesByHotelId(
        Guid hotelId, 
        int? offset = 0, 
        int? limit = 30, 
        string? orderBy = null, 
        bool ascending = true)
    {
        return await unitOfWork.RoomTypeRepository.GetRoomTypesByHotelIdAsync(
            hotelId, 
            offset, 
            limit, 
            orderBy, 
            ascending);
    }


    public async Task<RoomType> AddRoomType(RoomType roomType)
    {
        RoomType result = await unitOfWork.RoomTypeRepository.AddAsync(roomType);
        await unitOfWork.SaveAsync();
        return result;
    }
}