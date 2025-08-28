// RentalDto - DTO for rentals in Application layer
// Properties: Id, Motorcycle, Courier, PlanType, StartDate, EndDate, TotalCost, FineAmount, AdditionalDaysCost, AdditionalDays
using Moto.Domain.Enums;

namespace Moto.Application.DTOs.Rentals;

public class RentalDto
{
    public Guid Id { get; set; }
    public Guid MotorcycleId { get; set; }
    public Guid CourierId { get; set; }
    public RentalPlan PlanType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? TotalCost { get; set; }
    public decimal? FineAmount { get; set; }
    public decimal? AdditionalDaysCost { get; set; }
    public int? AdditionalDays { get; set; }
}