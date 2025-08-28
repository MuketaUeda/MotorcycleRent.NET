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
    /// Obtém um entregador pelo ID
    /// </summary>
    /// <param name="id">ID do entregador</param>
    /// <returns>Entregador encontrado ou null se não encontrado</returns>
    Task<CourierDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém todos os entregadores
    /// </summary>
    /// <returns>Lista de entregadores</returns>
    Task<IEnumerable<CourierDto>> GetAllAsync();

    /// <summary>
    /// Atualiza um entregador
    /// </summary>
    /// <param name="id">ID do entregador</param>
    /// <param name="request">Dados atualizados do entregador</param>
    /// <returns>Entregador atualizado</returns>
    Task<CourierDto> UpdateAsync(Guid id, UpdateCourierDto request);

    /// <summary>
    /// Remove um entregador
    /// </summary>
    /// <param name="id">ID do entregador a ser removido</param>
    /// <returns>True se removido com sucesso, false caso contrário</returns>
    Task<bool> DeleteAsync(Guid id);
}
