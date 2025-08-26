// CreateMotorcycleDto - DTO for creating a motorcycle
// Properties: Model, LicensePlate, Year
namespace Moto.Api.DTOs.Motorcycles;

// DTO for creating a motorcycle
public class CreateMotorcycleDto
{
    public required string Model { get; set; }
    public required string LicensePlate { get; set; }
    public int Year { get; set; }
}