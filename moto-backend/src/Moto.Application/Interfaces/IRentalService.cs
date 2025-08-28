// IRentalService - Interface do serviço de locações
// Define contratos para operações de negócio de locações
using Moto.Application.DTOs.Rentals;

namespace Moto.Application.Interfaces;

public interface IRentalService
{
    /// <summary>
    /// Cria uma nova locação
    /// </summary>
    /// <param name="createRentalDto">Dados da locação a ser criada</param>
    /// <returns>Locação criada</returns>
    Task<RentalDto> CreateAsync(CreateRentalDto createRentalDto);

    /// <summary>
    /// Obtém uma locação pelo ID
    /// </summary>
    /// <param name="id">ID da locação</param>
    /// <returns>Locação encontrada ou null se não encontrada</returns>
    Task<RentalDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém todas as locações
    /// </summary>
    /// <returns>Lista de locações</returns>
    Task<IEnumerable<RentalDto>> GetAllAsync();

    /// <summary>
    /// Finaliza uma locação (devolução)
    /// </summary>
    /// <param name="id">ID da locação</param>
    /// <param name="returnRentalDto">Dados da devolução</param>
    /// <returns>Locação finalizada</returns>
    Task<RentalDto> ReturnAsync(Guid id, ReturnRentalDto returnRentalDto);
}
