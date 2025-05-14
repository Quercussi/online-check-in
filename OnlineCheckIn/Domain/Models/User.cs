using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCheckIn.Domain.Models;

public class User : BaseModel
{
    public Guid Id { get; set; }
    
    [MaxLength(63)]
    public required string Username { get; set; }
    
    [MaxLength(63)]
    public required string Name { get; set; }
    
    [MaxLength(63)]
    public required string Email { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public virtual Company Company { get; set; } = null!;
    public virtual Guid CompanyId { get; set; }
}