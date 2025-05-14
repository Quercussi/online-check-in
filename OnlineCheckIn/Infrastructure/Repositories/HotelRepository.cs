using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;

namespace OnlineCheckIn.Infrastructure.Repositories;

public class HotelRepository(OnlineCheckInContext onlineCheckInContext): 
    BaseRepository<Hotel>(onlineCheckInContext), IHotelRepository
{
}