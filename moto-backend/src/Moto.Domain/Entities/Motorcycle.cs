using System.ComponentModel.DataAnnotations;

namespace Moto.Domain.Entities;

public class Motorcycle
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Identifier { get; set; } = string.Empty;
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;
    
    [Required]
    [StringLength(10)]
    public string Plate { get; set; } = string.Empty;
    
    public bool IsAvailable { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
