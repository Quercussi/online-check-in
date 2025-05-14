using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;

namespace OnlineCheckIn.Infrastructure.Repositories;

public class CompanyRepository(OnlineCheckInContext onlineCheckInContext)
    : BaseRepository<Company>(onlineCheckInContext), ICompanyRepository;