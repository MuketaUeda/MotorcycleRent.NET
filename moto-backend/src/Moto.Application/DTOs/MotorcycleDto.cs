// MotorcycleResponse - DTO for motorcycle response in Application layer
namespace Moto.Application.DTOs;

public class MotorcycleResponse
{
    public Guid Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public int Year { get; set; }
}
