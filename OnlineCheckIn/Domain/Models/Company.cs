using System.ComponentModel.DataAnnotations;

namespace OnlineCheckIn.Domain.Models;

public class Company : BaseModel
{
    public Guid Id { get; set; }
    
    [MaxLength(127)]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    public required string Address { get; set; }

    public virtual ICollection<Hotel> Hotels { get; set; } = null!;
}