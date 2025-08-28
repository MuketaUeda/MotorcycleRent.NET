// CreateRentalDto - DTO for creating a rental in Application layer
// Properties: MotorcycleId, CourierId, PlanType
using Moto.Domain.Enums;

namespace Moto.Application.DTOs.Rentals;

public class CreateRentalDto
{
    public required Guid MotorcycleId { get; set; }
    public required Guid CourierId { get; set; }
    public required RentalPlan PlanType { get; set; }
}
