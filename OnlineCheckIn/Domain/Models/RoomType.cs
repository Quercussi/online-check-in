using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineCheckIn.Domain.Models;

public class RoomType : BaseModel
{
    public Guid Id { get; set; }
    
    [MaxLength(31)]
    public required string Name { get; set; }
    
    public int MaxOccupancy { get; set; }
    
    [ForeignKey(nameof(HotelId))]
    [JsonIgnore]
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual Guid HotelId { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Room> Rooms { get; set; } = null!;
}