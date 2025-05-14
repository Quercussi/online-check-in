using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public interface ICompanyService
{
    Task<Company?> GetCompanyById(Guid id);
    Task<IEnumerable<Company>> GetAllCompanies();
    Task<Company> AddCompany(Company company);
}