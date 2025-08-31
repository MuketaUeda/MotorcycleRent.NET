// IRentalRepository - Interface of the rental repository
// Define contracts for rental persistence operations
using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface IRentalRepository
{
    /// Add a rental to the database
    Task<Rental> AddAsync(Rental rental);

    /// Search for a rental by ID
    Task<Rental?> GetByIdAsync(Guid id);

    /// Search for all active rentals by motorcycle ID
    Task<IEnumerable<Rental>> GetActiveRentalsByMotorcycleIdAsync(string motorcycleId);

    /// Update a rental
    Task<Rental> UpdateAsync(Rental rental);
}

