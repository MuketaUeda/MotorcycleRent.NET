using System.ComponentModel.DataAnnotations;
using Moto.Domain.Enums;

namespace Moto.Domain.Entities;

public class Rental
{
    public Guid Id { get; set; }
    
    [Required]
    public string MotorcycleId { get; set; } = string.Empty;
    
    [Required]
    public string CourierId { get; set; } = string.Empty;
    
    [Required]
    public RentalPlan PlanType { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime ExpectedEndDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    // Costs calculated
    public decimal? TotalCost { get; set; }
    
    public decimal? FineAmount { get; set; } // Fine for early return
    
    public decimal? AdditionalDaysCost { get; set; } // Cost for additional days
    
    public int? AdditionalDays { get; set; } // Total of additional days
    
    // Navigation properties
    public virtual Motorcycle Motorcycle { get; set; } = null!;
    public virtual Courier Courier { get; set; } = null!;
}
