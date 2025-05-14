using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCheckIn.Domain.Models;

public class Floor
{
    public Guid Id { get; private set; }
    public int Number { get; set; }
    
    [ForeignKey(nameof(HotelId))]
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual Guid HotelId { get; set; }
    
    public virtual ICollection<Room> Rooms { get; set; } = null!;
}