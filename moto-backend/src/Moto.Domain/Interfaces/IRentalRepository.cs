using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface IRentalRepository
{
    /// <summary>
    /// Adiciona uma nova locação
    /// </summary>
    /// <param name="rental">Locação a ser adicionada</param>
    /// <returns>Locação adicionada</returns>
    Task<Rental> AddAsync(Rental rental);

    /// <summary>       
    /// Obtém uma locação pelo ID
    /// </summary>
    /// <param name="id">ID da locação</param>
    /// <returns>Locação encontrada ou null se não encontrada</returns>
    Task<Rental?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém todas as locações
    /// </summary>
    /// <returns>Lista de locações</returns>
    Task<IEnumerable<Rental>> GetAllAsync();

    /// <summary>
    /// Obtém todas as locações ativas de uma motocicleta
    /// </summary>
    /// <param name="motorcycleId">ID da motocicleta</param>
    /// <returns>Lista de locações ativas</returns>
    Task<IEnumerable<Rental>> GetActiveRentalsByMotorcycleIdAsync(Guid motorcycleId);

    /// <summary>
    /// Atualiza uma locação
    /// </summary>
    /// <param name="rental">Locação a ser atualizada</param>
    /// <returns>Locção atualizada</returns>
    Task<Rental> UpdateAsync(Rental rental);
}

