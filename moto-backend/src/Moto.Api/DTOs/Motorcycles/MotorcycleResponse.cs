// MotorcycleResponse - DTO for motorcycle responses in API layer

namespace Moto.Api.DTOs.Motorcycles;

public class MotorcycleResponse
{
    public required string Id { get; set; }
    public required string Model { get; set; }
    public required string Plate { get; set; }
    public required int Year { get; set; }
}
