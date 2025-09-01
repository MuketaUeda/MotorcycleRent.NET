// RentalResponse - DTO for complete details of rentals in API layer
// Properties: Id, Motorcycle, Courier, PlanType, StartDate, EndDate, TotalCost, FineAmount, AdditionalDaysCost, AdditionalDays
using Moto.Domain.Enums;

namespace Moto.Api.DTOs.Rentals;

public class RentalResponse
{
    public Guid Id { get; set; }
    public string MotorcycleId { get; set; } = string.Empty;
    public string CourierId { get; set; } = string.Empty;
    public RentalPlan PlanType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? TotalCost { get; set; }
    public decimal? FineAmount { get; set; }
    public decimal? AdditionalDaysCost { get; set; }
    public int? AdditionalDays { get; set; }
}
