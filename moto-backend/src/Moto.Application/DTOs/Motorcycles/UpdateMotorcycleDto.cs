// UpdateMotorcycleDto - DTO for updating a motorcycle in Application layer
namespace Moto.Application.DTOs.Motorcycles;

public class UpdateMotorcycleDto
{
    public string? Model { get; set; }
    public string? Plate { get; set; }
    public int? Year { get; set; }
}
