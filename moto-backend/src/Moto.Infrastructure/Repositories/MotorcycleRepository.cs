// MotorcycleRepository - Implement repository for motorcycles
// Implement IMotorcycleRepository using Entity Framework
using Microsoft.EntityFrameworkCore;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

public class MotorcycleRepository : IMotorcycleRepository{
    private readonly ApplicationDbContext _context;

    public MotorcycleRepository(ApplicationDbContext context){
        _context = context;
    }

    /// Add a motorcycle to the database
    public async Task<Motorcycle> AddAsync(Motorcycle motorcycle){
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();
        return motorcycle;
    }

    /// Search for a motorcycle by ID 
    public async Task<Motorcycle?> GetByIdAsync(string id){
        return await _context.Motorcycles.FindAsync(id);
    }


    /// Search for a motorcycle by plate (Validation of plate)
    public async Task<Motorcycle?> GetByPlateAsync(string plate){
        return await _context.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate);
    }

    /// Search for all motorcycles
    public async Task<IReadOnlyList<Motorcycle>> GetAllAsync(){
        return await _context.Motorcycles.ToListAsync();
    }

    /// Search for all and filtered motorcycles by plate
    public async Task<IReadOnlyList<Motorcycle>> GetByPlateFilterAsync(string? plateFilter)
    {
        if (string.IsNullOrWhiteSpace(plateFilter))
        {
            return await _context.Motorcycles.ToListAsync();
        }

        return await _context.Motorcycles
            .Where(m => m.Plate.Contains(plateFilter.ToUpper()))
            .ToListAsync();
    }

    /// Update an existing motorcycle
    public async Task<Motorcycle> UpdateAsync(Motorcycle motorcycle){
        _context.Motorcycles.Update(motorcycle);
        await _context.SaveChangesAsync();
        return motorcycle;
    }

    /// Remove a motorcycle by ID
    public async Task<bool> DeleteAsync(string id){
        var motorcycle = await GetByIdAsync(id);
        if (motorcycle != null){
            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}