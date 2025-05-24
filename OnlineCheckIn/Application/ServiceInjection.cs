using OnlineCheckIn.Application.Services;

namespace OnlineCheckIn.Application;

public static class ServiceInjection
{
    public static IServiceCollection AddService
        (this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IRoomTypeService, RoomTypeService>();
        services.AddScoped<IRoomService, RoomService>();
    
        return services;
    }
}