using System.ComponentModel.DataAnnotations;

namespace Moto.Domain.Entities;

public class MotorcycleEvent
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid MotorcycleId { get; set; }
    
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
    [StringLength(50)]
    public string MotorcyclePlate { get; set; } = string.Empty;
    
    public string? AdditionalData { get; set; }
    
    // Navigation property
    public virtual Motorcycle Motorcycle { get; set; } = null!;
}
