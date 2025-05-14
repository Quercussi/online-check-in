using System.ComponentModel.DataAnnotations;

namespace OnlineCheckIn.Domain.Models;

public class Customer : BaseModel
{
    public Guid Id { get; set; }
    
    [MaxLength(63)]
    public required string Name { get; set; }
    
    [MaxLength(63)]
    public string? Email { get; set; }
    
    [MaxLength(31)]
    public string? PhoneNumber { get; set; }
    
    [MaxLength(255)]
    public string? Address { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public virtual ICollection<Reservation> Reservations { get; set; }
}