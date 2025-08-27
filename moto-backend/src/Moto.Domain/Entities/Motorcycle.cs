using System.ComponentModel.DataAnnotations;

namespace Moto.Domain.Entities;

public class Motorcycle
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Plate { get; set; } = string.Empty;
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;
}
