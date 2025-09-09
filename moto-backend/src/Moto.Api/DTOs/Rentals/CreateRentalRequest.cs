// CreateRentalRequest - DTO for creating rentals in API layer
// Properties: MotorcycleId, CourierId, StartDate, EndDate, ExpectedEndDate, PlanType
using Moto.Domain.Enums;

namespace Moto.Api.DTOs.Rentals;

public class CreateRentalRequest
{
    public required string MotorcycleId { get; set; }
    public required string CourierId { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public required DateTime ExpectedEndDate { get; set; }
    public required RentalPlan PlanType { get; set; }
}
