using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Booking
{
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "You must enter an employee name.")]
    public string EmployeeName { get; set; }

    [Required(ErrorMessage = "You must enter a start time.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "You must enter an end time.")]
    public DateTime EndTime { get; set; }

    public string? Notes { get; set; }

    [ForeignKey("Resource")] [Required] public int ResourceId { get; set; }

    public Resource Resource { get; set; }
}