using System.ComponentModel.DataAnnotations;
using Moto.Domain.Enums;

namespace Moto.Domain.Entities;

public class Courier
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Identifier { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(18)]
    public string Cnpj { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [StringLength(11)]
    public string LicenseNumber { get; set; } = string.Empty;
    
    [Required]
    public CnhType LicenseType { get; set; }
    
    public string? LicenseImageUrl { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
