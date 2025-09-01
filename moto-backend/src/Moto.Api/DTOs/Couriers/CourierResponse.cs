// CourierResponse - DTO for complete details of couriers
// Properties: Id, Name, CNPJ, BirthDate, CnhNumber, CnhType, CnhImageUrl
using Moto.Domain.Enums;

namespace Moto.Api.DTOs.Couriers;

public class CourierResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Cnpj { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string CnhNumber { get; set; }
    public required CnhType CnhType { get; set; }
    public string? CnhImageUrl { get; set; }
}
