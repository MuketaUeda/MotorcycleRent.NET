// UpdateMotorcycleRequest - DTO para atualização de motos
// Propriedades: Placa (única propriedade editável)
namespace Moto.Api.DTOs.Motorcycles;

public class UpdateMotorcycleRequest
{
    public required string Plate { get; set; }
}
