// UpdateMotorcycleRequest - DTO for updating motorcycles
// Properties: Plate (only editable property)
namespace Moto.Api.DTOs.Motorcycles;

public class UpdateMotorcycleRequest
{
    public required string Plate { get; set; }
}
