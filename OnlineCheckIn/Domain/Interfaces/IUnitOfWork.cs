namespace OnlineCheckIn.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository CompanyRepository { get; }

    Task<int> SaveAsync();   
}