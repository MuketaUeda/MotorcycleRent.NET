using Moto.Domain.Entities;

namespace Moto.Domain.Interfaces;

public interface IMotorcycleEventRepository
{
    Task<MotorcycleEvent> AddAsync(MotorcycleEvent motorcycleEvent);
    Task<IEnumerable<MotorcycleEvent>> GetAllAsync();
    Task<IEnumerable<MotorcycleEvent>> GetByYearAsync(int year);
}
