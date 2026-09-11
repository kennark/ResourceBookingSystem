using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Resource
{
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "You must enter a name for the resource.")]
    public string Name { get; set; }

    [Required] public bool IsActive { get; set; } = true;

    public ICollection<Booking> Bookings { get; set; }
}