// IMotorcycleEventRepository - Interface for motorcycle event repository operations
using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

/// Repository interface for motorcycle event operations
public interface IMotorcycleEventRepository
{
    /// Add a new motorcycle event to the database
    Task<MotorcycleEvent> AddAsync(MotorcycleEvent motorcycleEvent);
}
