// IMotorcycleEventRepository - Interface for motorcycle event repository operations
using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

/// <summary>
/// Repository interface for motorcycle event operations
/// </summary>
public interface IMotorcycleEventRepository
{
    /// <summary>
    /// Add a new motorcycle event to the database
    /// </summary>
    /// <param name="motorcycleEvent">The motorcycle event to add</param>
    /// <returns>The added motorcycle event</returns>
    Task<MotorcycleEvent> AddAsync(MotorcycleEvent motorcycleEvent);

    /// <summary>
    /// Get all motorcycle events ordered by event date (newest first)
    /// </summary>
    /// <returns>Collection of motorcycle events</returns>
    Task<IEnumerable<MotorcycleEvent>> GetAllAsync();

    /// <summary>
    /// Get motorcycle events by year
    /// </summary>
    /// <param name="year">The year to filter by</param>
    /// <returns>Collection of motorcycle events for the specified year</returns>
    Task<IEnumerable<MotorcycleEvent>> GetByYearAsync(int year);
}
