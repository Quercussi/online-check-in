using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCheckIn.Domain.Models;

public class Hotel : BaseModel
{
    public Guid Id { get; set; }
    
    [MaxLength(127)]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    public required string Address { get; set; }
    
    [MaxLength(31)]
    public required string PhoneNumber { get; set; }
    
    [MaxLength(63)]
    public required string Email { get; set; }
    
    [ForeignKey(nameof(CompanyId))]
    public virtual Company Company { get; set; } = null!;
    public Guid CompanyId { get; set; }
    
    public virtual ICollection<RoomType> RoomTypes { get; set; } = null!;
    public virtual ICollection<Floor> Floors { get; set; } = null!;
}