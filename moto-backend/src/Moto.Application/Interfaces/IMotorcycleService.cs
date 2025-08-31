// IMotorcycleService - Interface do serviço de motocicletas
// Define contratos para operações de negócio de motocicletas
using Moto.Application.DTOs.Motorcycles;

namespace Moto.Application.Interfaces;

public interface IMotorcycleService
{
    /// <summary>
    /// Cria uma nova motocicleta
    /// </summary>
    /// <param name="request">Dados da motocicleta a ser criada</param>
    /// <returns>Motocicleta criada</returns>
    Task<MotorcycleDto> CreateAsync(CreateMotorcycleDto request);

    /// <summary>
    /// Obtém uma motocicleta pelo ID
    /// </summary>
    /// <param name="id">ID da motocicleta</param>
    /// <returns>Motocicleta encontrada ou null se não encontrada</returns>
    Task<MotorcycleDto?> GetByIdAsync(string id);

    /// <summary>
    /// Obtém todas as motocicletas com filtro opcional por placa
    /// </summary>
    /// <param name="plateFilter">Filtro de placa opcional (busca parcial)</param>
    /// <returns>Lista de motocicletas que correspondem ao filtro</returns>
    Task<IEnumerable<MotorcycleDto>> GetAllAsync(string? plateFilter = null);

    /// <summary>
    /// Atualiza uma motocicleta
    /// </summary>
    /// <param name="id">ID da motocicleta</param>
    /// <param name="request">Dados atualizados da motocicleta</param>
    /// <returns>Motocicleta atualizada</returns>
    Task<MotorcycleDto> UpdateAsync(string id, UpdateMotorcycleDto request);

    /// <summary>
    /// Remove uma motocicleta
    /// </summary>
    /// <param name="id">ID da motocicleta a ser removida</param>
    /// <returns>True se removida com sucesso, false caso contrário</returns>
    Task<bool> DeleteAsync(string id);
}
