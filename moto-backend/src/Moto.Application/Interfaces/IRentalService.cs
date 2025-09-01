// IRentalService - Interface of the rental service
// Define contracts for rental business operations
using Moto.Application.DTOs.Rentals;

namespace Moto.Application.Interfaces;

public interface IRentalService
{
    /// Create a new rental
    Task<RentalDto> CreateAsync(CreateRentalDto createRentalDto);

    /// Get a rental by ID
    Task<RentalDto?> GetByIdAsync(Guid id);

    /// Finalize a rental (return)
    Task<RentalDto> ReturnAsync(Guid id, ReturnRentalDto returnRentalDto);
}
