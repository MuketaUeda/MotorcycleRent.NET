// CreateMotorcycleDto - DTO for creating a motorcycle in Application layer
namespace Moto.Application.DTOs.Motorcycles;

public class CreateMotorcycleDto
{
    public required string Model { get; set; }
    public required string Plate { get; set; }
    public required int Year { get; set; }
}
