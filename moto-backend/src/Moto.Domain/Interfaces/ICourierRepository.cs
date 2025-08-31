// ICourierRepository - Interface of the courier repository
// Define contracts for courier persistence operations
using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface ICourierRepository
{
    /// Add a courier to the database
    Task<Courier> AddAsync(Courier courier);

    /// Search for a courier by ID
    Task<Courier?> GetByIdAsync(string id);

    /// Search for a courier by CNPJ
    Task<Courier?> GetByCnpjAsync(string cnpj);

    /// Search for a courier by CNH number
    Task<Courier?> GetByCnhNumberAsync(string cnhNumber);

    /// Update an existing courier
    Task<Courier> UpdateAsync(Courier courier);
}