// ICourierRepository - Interface do repositório de entregadores
// Define contratos para operações de persistência de entregadores
using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface ICourierRepository{
    /// <summary>
    /// Add a courier to the database
    /// </summary>
    Task<Courier> AddAsync(Courier courier);

    /// <summary>
    /// Search for a courier by ID
    /// </summary>
    /// <param name="id">ID of the courier</param>
    /// <returns>Found courier or null if not found</returns>
    Task<Courier?> GetByIdAsync(string id);

    /// <summary>
    /// Search for a courier by CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ of the courier</param>
    /// <returns>Found courier or null if not found</returns>
    Task<Courier?> GetByCnpjAsync(string cnpj);

    /// <summary>
    /// Search for a courier by CNH number
    /// </summary>
    /// <param name="cnhNumber">CNH number of the courier</param>
    /// <returns>Found courier or null if not found</returns>
    Task<Courier?> GetByCnhNumberAsync(string cnhNumber);

    /// <summary>
    /// Search for all couriers
    /// </summary>
    /// <returns>List of all couriers</returns>
    Task<IReadOnlyList<Courier>> GetAllAsync();

    /// <summary>
    /// Update an existing courier
    /// </summary>
    /// <param name="courier">Courier with updated data</param>
    /// <returns>Updated courier</returns>
    Task<Courier> UpdateAsync(Courier courier);

    /// <summary>
    /// Remove a courier by ID
    /// </summary>
    /// <param name="id">ID of the courier to be removed</param>
    /// <returns>True if removed successfully, false if not found</returns>
    Task<bool> DeleteAsync(string id);

}