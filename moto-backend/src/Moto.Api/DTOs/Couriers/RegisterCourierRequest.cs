// RegisterCourierRequest - DTO for registering couriers
// Properties: identifier, name, cnpj, birthDate, licenseNumber, licenseType, licenseImage
using Moto.Domain.Enums;

namespace Moto.Api.DTOs.Couriers;

public class RegisterCourierRequest
{
    public required string Identifier { get; set; }
    public required string Name { get; set; }
    public required string Cnpj { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string LicenseNumber { get; set; }
    public required CnhType LicenseType { get; set; }
    public required string LicenseImage { get; set; }
}