namespace OnlineCheckIn.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository CompanyRepository { get; }
    IHotelRepository HotelRepository { get; }

    Task<int> SaveAsync();   
}