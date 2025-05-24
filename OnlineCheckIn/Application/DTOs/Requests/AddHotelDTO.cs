namespace OnlineCheckIn.Application.DTOs.Requests;

public class AddHotelDTO
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid CompanyId { get; set; }
}