using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Infrastructure.Database;

namespace OnlineCheckIn.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly OnlineCheckInContext _context;

    private readonly Lazy<ICompanyRepository> _companyRepository;

    public UnitOfWork(OnlineCheckInContext context)
    {
        _context = context;
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(_context));
    }

    public ICompanyRepository CompanyRepository => _companyRepository.Value;

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    private bool _disposed = false;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}