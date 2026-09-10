using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Resource
{
    [Key] public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public bool IsActive { get; set; }
    
    public ICollection<Booking> Bookings { get; set; }
}