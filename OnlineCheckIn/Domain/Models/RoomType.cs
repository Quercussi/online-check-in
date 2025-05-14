using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCheckIn.Domain.Models;

public class RoomType : BaseModel
{
    public Guid Id { get; set; }
    
    [MaxLength(31)]
    public required string Name { get; set; }
    
    public int MaxOccupancy { get; set; }
    
    [ForeignKey(nameof(HotelId))]
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual Guid HotelId { get; set; }
    
    public virtual ICollection<Room> Rooms { get; set; } = null!;
}