// RegisterCourierRequest - DTO for registering couriers
// Properties: id, name, cnpj, birthDate, cnhNumber, cnhType, cnhImageUrl
using Moto.Domain.Enums;

namespace Moto.Api.DTOs.Couriers;

public class RegisterCourierRequest
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Cnpj { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string CnhNumber { get; set; }
    public required CnhType CnhType { get; set; }
    public required string CnhImageUrl { get; set; }
}