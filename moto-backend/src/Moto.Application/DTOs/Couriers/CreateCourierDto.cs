// CreateCourierDto - DTO for creating a courier in Application layer
using Moto.Domain.Enums;

namespace Moto.Application.DTOs.Couriers;

public class CreateCourierDto
{
    public required string Name { get; set; }
    public required string Cnpj { get; set; }
    public required int BirthDate { get; set; }
    public required string CnhNumber { get; set; }
    public required CnhType CnhType { get; set; }
    public required string CnhImageUrl { get; set; }
}

