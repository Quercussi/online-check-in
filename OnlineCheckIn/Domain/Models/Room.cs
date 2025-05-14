using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCheckIn.Domain.Models;

public class Room : BaseModel
{
    public Guid Id { get; set; }
    [MaxLength(7)]
    public required string Number { get; set; }
    
    [ForeignKey(nameof(FloorId))]
    public virtual Floor Floor { get; set; } = null!;
    public virtual Guid FloorId { get; set; }
    
    [ForeignKey(nameof(RoomTypeId))]
    public virtual RoomType RoomType { get; set; } = null!;
    public virtual Guid RoomTypeId { get; set; }
    
    public virtual ICollection<Schedule> Schedules { get; set; } = null!;
}