// MotorcycleDto - DTO for motorcycle responses in Application layer

namespace Moto.Application.DTOs.Motorcycles;

public class MotorcycleDto
{
    public required string Id { get; set; }
    public required string Model { get; set; }
    public required string Plate { get; set; }
    public required int Year { get; set; }
}
