using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineCheckIn.Domain.Models;

public class Room : BaseModel
{
    public Guid Id { get; set; }
    [MaxLength(7)]
    public required string Number { get; set; }
    
    public virtual Int16 FloorNumber { get; set; }
    
    [ForeignKey(nameof(RoomTypeId))]
    [JsonIgnore]
    public virtual RoomType RoomType { get; set; } = null!;
    public virtual Guid RoomTypeId { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Schedule> Schedules { get; set; } = null!;
}