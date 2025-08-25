// IMotorcycleRepository - Interface do repositório de motos
// Define contratos para operações de persistência de motos
using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface IMotorcycleRepository
{
    /// <summary>
    /// Add a motorcycle to the database
    /// </summary>
    /// <param name="motorcycle">Motorcycle to be added</param>
    /// <returns>Motorcycle added with generated ID</returns>
    Task<Motorcycle> AddAsync(Motorcycle motorcycle);

    /// <summary>
    /// Search for a motorcycle by ID
    /// </summary>
    /// <param name="id">ID of the motorcycle</param>
    /// <returns>Found motorcycle or null if not found</returns>
    Task<Motorcycle?> GetByIdAsync(Guid id);

    /// <summary>
    /// Search for a motorcycle by plate
    /// </summary>
    /// <param name="licensePlate">Plate of the motorcycle</param>
    /// <returns>Found motorcycle or null if not found</returns>
    Task<Motorcycle?> GetByPlateAsync(string licensePlate);

    /// <summary>
    /// Search for all motorcycles
    /// </summary>
    /// <returns>List of all motorcycles</returns>
    Task<IReadOnlyList<Motorcycle>> GetAllAsync();

    /// <summary>
    /// Update an existing motorcycle
    /// </summary>
    /// <param name="motorcycle">Motorcycle with updated data</param>
    /// <returns>Updated motorcycle</returns>
    Task<Motorcycle> UpdateAsync(Motorcycle motorcycle);

    /// <summary>
    /// Remove a motorcycle by ID
    /// </summary>
    /// <param name="id">ID of the motorcycle to be removed</param>
    /// <returns>True if removed successfully, false if not found</returns>
    Task<bool> DeleteAsync(Guid id);
}