// CreateRentalRequest - DTO for creating rentals
// Properties: courierId, motorcycleId, startDate, endDate, expectedEndDate, plan
using Moto.Domain.Enums;

namespace Moto.Api.DTOs.Rentals;

public class CreateRentalRequest
{
    public required string CourierId { get; set; }
    public required string MotorcycleId { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required DateTime ExpectedEndDate { get; set; }
    public required int Plan { get; set; }
}
