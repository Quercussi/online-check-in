using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Repositories;

namespace OnlineCheckIn.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructure
        (this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: use connection string from app config instead
        string connectionString =
            "Host=localhost;Port=5432;Username=online_hotel_check_in_user;Password=online_hotel_check_in_password_dev;Database=online_hotel_check_in_db;";

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<OnlineCheckInContext>(optionsBuilder => optionsBuilder.UseNpgsql(connectionString));
        
        return services;
    }
}