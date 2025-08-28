// CreateRentalRequest - DTO for creating rentals
// Properties: MotorcycleId, CourierId, PlanType
using Moto.Domain.Enums;

namespace Moto.Api.DTOs.Rentals;

public class CreateRentalRequest
{
    public required Guid MotorcycleId { get; set; }
    public required Guid CourierId { get; set; }
    public required RentalPlan PlanType { get; set; }
}
