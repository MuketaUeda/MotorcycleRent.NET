using System.ComponentModel.DataAnnotations;
using Moto.Domain.Enums;

namespace Moto.Domain.Entities;

public class Courier
{
    [Required]
    [StringLength(50)]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    [StringLength(14)]
    public string Cnpj { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [StringLength(11)]
    public string CnhNumber { get; set; } = string.Empty;
    
    [Required]
    public CnhType CnhType { get; set; }
    
    public string? CnhImageUrl { get; set; }
}
