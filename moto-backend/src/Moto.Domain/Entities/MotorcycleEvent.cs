// MotorcycleEvent - Entity for motorcycle events
// Represents a motorcycle event in the database
using System.ComponentModel.DataAnnotations;

namespace Moto.Domain.Entities;

public class MotorcycleEvent
{
    public Guid Id { get; set; }
    
    [Required]
    public string MotorcycleId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string EventType { get; set; } = string.Empty;
    
    [Required]
    public DateTime EventDate { get; set; }
    
    [Required]
    public int MotorcycleYear { get; set; }
    
    [Required]
    [StringLength(100)]
    public string MotorcycleModel { get; set; } = string.Empty;
    
    [Required]
    [StringLength(7)]
    public string MotorcyclePlate { get; set; } = string.Empty;
}
