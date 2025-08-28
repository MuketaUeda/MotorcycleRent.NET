// CourierDto - DTO for courier response in Application layer
using Moto.Domain.Enums;

namespace Moto.Application.DTOs.Couriers;

public class CourierDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; } = string.Empty;
    public CnhType CnhType { get; set; }
    public string? CnhImageUrl { get; set; }
}
