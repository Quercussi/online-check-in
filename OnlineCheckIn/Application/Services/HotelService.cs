using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public class HotelService(IUnitOfWork unitOfWork) : IHotelService
{
    public async Task<Hotel?> GetHotelById(Guid id)
    {
        return await unitOfWork.HotelRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Hotel>> GetHotelsByCompanyId(
        Guid companyId, 
        int? offset = 0, 
        int? limit = 30, 
        string? orderBy = null, 
        bool ascending = true)
    {
        return await unitOfWork.HotelRepository.GetHotelsByCompanyIdAsync(
            companyId, 
            offset, 
            limit, 
            orderBy, 
            ascending);
    }


    public async Task<Hotel> AddHotel(Hotel hotel)
    {
        Hotel result = await unitOfWork.HotelRepository.AddAsync(hotel);
        await unitOfWork.SaveAsync();
        return result;
    }
}