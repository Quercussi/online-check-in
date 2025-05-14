using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCheckIn.Domain.Models;

public class Reservation : BaseModel
{
    public Guid Id { get; private set; }
    
    public DateTime ReservationDate { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int OccupantCount { get; set; }
    
    [ForeignKey(nameof(RoomTypeId))]
    public virtual RoomType RoomType { get; set; } = null!;
    public virtual Guid RoomTypeId { get; set; }
    
    
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;
    public virtual Guid CustomerId { get; set; }
    
    [ForeignKey(nameof(ScheduleId))]
    public virtual Schedule? Schedules { get; set; }
    public virtual Guid? ScheduleId { get; set; }
}