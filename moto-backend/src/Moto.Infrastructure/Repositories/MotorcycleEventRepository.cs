using Microsoft.EntityFrameworkCore;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

public class MotorcycleEventRepository : IMotorcycleEventRepository
{
    private readonly ApplicationDbContext _context;

    public MotorcycleEventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MotorcycleEvent> AddAsync(MotorcycleEvent motorcycleEvent)
    {
        _context.MotorcycleEvents.Add(motorcycleEvent);
        await _context.SaveChangesAsync();
        return motorcycleEvent;
    }

    public async Task<IEnumerable<MotorcycleEvent>> GetAllAsync()
    {
        return await _context.MotorcycleEvents
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MotorcycleEvent>> GetByYearAsync(int year)
    {
        return await _context.MotorcycleEvents
            .Where(e => e.MotorcycleYear == year)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }
}
