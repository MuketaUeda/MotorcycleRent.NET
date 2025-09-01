// IMotorcycleService - Interface of the motorcycle service
// Define contracts for motorcycle business operations  
using Moto.Application.DTOs.Motorcycles;

namespace Moto.Application.Interfaces;

public interface IMotorcycleService
{
    /// Create a new motorcycle
    Task<MotorcycleDto> CreateAsync(CreateMotorcycleDto request);

    /// Get a motorcycle by ID
    Task<MotorcycleDto?> GetByIdAsync(string id);

    /// Get all motorcycles with optional plate filter
    Task<IEnumerable<MotorcycleDto>> GetAllAsync(string? plateFilter = null);

    /// Update a motorcycle
    Task<MotorcycleDto> UpdateAsync(string id, UpdateMotorcycleDto request);

    /// Delete a motorcycle
    Task<bool> DeleteAsync(string id);
}
