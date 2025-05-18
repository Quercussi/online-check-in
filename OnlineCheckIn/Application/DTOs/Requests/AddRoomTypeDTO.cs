namespace OnlineCheckIn.Application.DTOs.Requests;

public class AddRoomTypeDTO
{
    public string Name { get; set; } = null!;
    public int MaxOccupancy { get; set; }
    public Guid HotelId { get; set; }
}