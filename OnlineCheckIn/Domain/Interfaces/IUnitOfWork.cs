namespace OnlineCheckIn.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository CompanyRepository { get; }
    IHotelRepository HotelRepository { get; }
    IRoomTypeRepository RoomTypeRepository { get; }
    IRoomRepository RoomRepository { get; }

    Task<int> SaveAsync();   
}