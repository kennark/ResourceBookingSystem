using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Booking
{
    [Key] public int Id { get; set; }

    [Required] public string EmployeeName { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }
    public string? Notes { get; set; }

    [ForeignKey("Resource")] [Required] public int ResourceId { get; set; }

    [Required] public Resource Resource { get; set; }
}