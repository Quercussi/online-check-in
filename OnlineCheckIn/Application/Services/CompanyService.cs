using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.Application.Services;

public class CompanyService(IUnitOfWork unitOfWork) : ICompanyService
{
    public async Task<Company?> GetCompanyById(Guid id)
    {
        return await unitOfWork.CompanyRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Company>> GetAllCompanies()
    {
        return await unitOfWork.CompanyRepository.GetAllAsync();
    }

    public async Task<Company> AddCompany(Company company)
    {
        Company result = await unitOfWork.CompanyRepository.AddAsync(company);
        await unitOfWork.SaveAsync();
        return result;
    }
}