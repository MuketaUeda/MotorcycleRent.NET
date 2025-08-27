// MotorcycleResponse - DTO for motorcycle data response in API layer
// Properties: Id, Year, Model, Plate, IsAvailable, CreatedAt, UpdatedAt
namespace Moto.Api.DTOs.Motorcycles;

public class MotorcycleResponse
{
    public Guid Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public required int Year { get; set; }
}
