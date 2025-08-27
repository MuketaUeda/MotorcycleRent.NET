// UpdateCourierRequest - DTO para atualização de perfil de entregadores
// Propriedades: Nome, ImagemCNH
namespace Moto.Api.DTOs.Couriers;

public class UpdateCourierRequest
{
    public string? Name { get; set; }
    public string? CnhImageUrl { get; set; }
}
