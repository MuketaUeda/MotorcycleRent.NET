// CourierResponse - DTO para detalhes completos de entregadores
// Propriedades: Id, Nome, CNPJ, DataNascimento, NumeroCNH, TipoCNH, ImagemCNH, Status
namespace Moto.Api.DTOs.Couriers;

public class CourierResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public int BirthDate { get; set; }
    public string CnhNumber { get; set; } = string.Empty;
    public string CnhType { get; set; } = string.Empty;
    public string? CnhImageUrl { get; set; }
}
