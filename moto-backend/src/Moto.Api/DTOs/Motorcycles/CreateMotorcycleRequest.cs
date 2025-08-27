// CreateMotorcycleRequest - DTO for creating a motorcycle in API layer
// Properties: Model, Plate, Year
namespace Moto.Api.DTOs.Motorcycles;
    
public class CreateMotorcycleRequest
{
    public required string Model { get; set; }
    public required string Plate { get; set; }
    public required int Year { get; set; }
}
