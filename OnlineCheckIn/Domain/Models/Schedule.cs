using System.ComponentModel.DataAnnotations.Schema;
using OnlineCheckIn.Domain.Enums;

namespace OnlineCheckIn.Domain.Models;

public class Schedule : BaseModel
{
    public Guid Id { get; set; }
    public ScheduleType ScheduleType { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public virtual Reservation? Reservation { get; set; }
    public virtual Guid ReservationId { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; } = null!;
    public virtual Guid RoomId { get; set; }
    
}