// MotorcycleDto - DTO for motorcycle response in Application layer
namespace Moto.Application.DTOs.Motorcycles;

public class MotorcycleDto
{
    public Guid Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public required int Year { get; set; }
}
