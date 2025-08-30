using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface IMotorcycleEventRepository
{
    Task<MotorcycleEvent> AddAsync(MotorcycleEvent motorcycleEvent);
    Task<IEnumerable<MotorcycleEvent>> GetByYearAsync(int year);
    Task<IEnumerable<MotorcycleEvent>> GetByMotorcycleIdAsync(Guid motorcycleId);
    Task<IEnumerable<MotorcycleEvent>> GetAllAsync();
}
