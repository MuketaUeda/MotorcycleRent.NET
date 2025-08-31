// ICourierService - Interface do serviço de entregadores
// Define contratos para operações de negócio de entregadores
using Moto.Application.DTOs.Couriers;

namespace Moto.Application.Interfaces;

public interface ICourierService
{
    /// <summary>
    /// Cria um novo entregador
    /// </summary>
    /// <param name="request">Dados do entregador a ser criado</param>
    /// <returns>Entregador criado</returns>
    Task<CourierDto> CreateAsync(CreateCourierDto request);

    /// <summary>
    /// Atualiza a foto da CNH de um entregador
    /// </summary>
    /// <param name="id">ID do entregador</param>
    /// <param name="request">Dados da foto da CNH</param>
    /// <returns>Entregador atualizado</returns>
    Task<CourierDto> UpdateCnhImageAsync(string id, UpdateCnhImageDto request);
}
