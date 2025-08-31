// CourierResponse - DTO for complete details of couriers
// Properties: Id, Name, CNPJ, BirthDate, LicenseNumber, LicenseType, LicenseImage
namespace Moto.Api.DTOs.Couriers;

public class CourierResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Cnpj { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string LicenseNumber { get; set; }
    public required string LicenseType { get; set; }
    public string? LicenseImage { get; set; }
}
