using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public interface IHotelService
{
    Task<Hotel?> GetCompanyById(Guid id);

    Task<IEnumerable<Hotel>> GetHotelsByCompanyId(
        Guid companyId,
        int? offset = 0,
        int? limit = 30,
        string? orderBy = null,
        bool ascending = true);
    
    Task<Hotel> AddHotel(Hotel hotel);
}