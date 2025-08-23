using System.ComponentModel.DataAnnotations;
using Moto.Domain.Enums;

namespace Moto.Domain.Entities;

public class Rental
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid MotorcycleId { get; set; }
    
    [Required]
    public Guid CourierId { get; set; }
    
    [Required]
    public RentalPlan PlanType { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    [Required]
    public decimal TotalCost { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual Motorcycle Motorcycle { get; set; } = null!;
    public virtual Courier Courier { get; set; } = null!;
}
