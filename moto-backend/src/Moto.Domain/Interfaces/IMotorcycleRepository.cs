// IMotorcycleRepository - Interface of the motorcycle repository
// Define contracts for motorcycle persistence operations
using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface IMotorcycleRepository
{
    /// Add a motorcycle to the database
    Task<Motorcycle> AddAsync(Motorcycle motorcycle);

    /// Search for a motorcycle by ID
    Task<Motorcycle?> GetByIdAsync(string id);

    /// Search for a motorcycle by plate
    Task<Motorcycle?> GetByPlateAsync(string plate);

    /// Search for motorcycles by plate filter
    Task<IReadOnlyList<Motorcycle>> GetByPlateFilterAsync(string? plateFilter);

    /// Update an existing motorcycle
    Task<Motorcycle> UpdateAsync(Motorcycle motorcycle);

    /// Remove a motorcycle by ID
    Task<bool> DeleteAsync(string id);
}