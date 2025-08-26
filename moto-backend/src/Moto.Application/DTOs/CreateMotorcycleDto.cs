// CreateMotorcycleRequest - DTO for creating a motorcycle in Application layer
namespace Moto.Application.DTOs;

public class CreateMotorcycleRequest
{
    public required string Model { get; set; }
    public required string LicensePlate { get; set; }
    public int Year { get; set; }
}
