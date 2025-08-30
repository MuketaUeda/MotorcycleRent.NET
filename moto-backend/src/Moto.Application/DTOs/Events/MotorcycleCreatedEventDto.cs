// DTO for the motorcycle created event

namespace Moto.Application.DTOs.Events;

public class MotorcycleCreatedEventDto
{
    public Guid Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public int Year { get; set; }
}