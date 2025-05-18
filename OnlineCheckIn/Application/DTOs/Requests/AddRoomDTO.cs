namespace OnlineCheckIn.Application.DTOs.Requests;

public class AddRoomDTO
{
    public string Number { get; set; } = null!;
    public short FloorNumber { get; set; }
    public Guid RoomTypeId { get; set; }
}