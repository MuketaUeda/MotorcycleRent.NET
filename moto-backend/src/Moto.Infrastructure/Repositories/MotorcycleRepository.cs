// MotorcycleRepository - Implementação do repositório de motos
// Implementa IMotorcycleRepository usando Entity Framework
using Microsoft.EntityFrameworkCore;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

/// <summary>
/// Implementation of the motorcycle repository using Entity Framework
/// </summary>
public class MotorcycleRepository : IMotorcycleRepository{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor that receives the ApplicationDbContext
    /// </summary>
    /// <param name="context">ApplicationDbContext</param>
    public MotorcycleRepository(ApplicationDbContext context){
        _context = context;
    }

    /// <summary>
    /// Add a motorcycle to the database
    /// </summary>
    /// <param name="motorcycle">Motorcycle to be added</param>
    /// <returns>Motorcycle added with generated ID</returns>
    public async Task<Motorcycle> AddAsync(Motorcycle motorcycle){
        await _context.Motorcycles.AddAsync(motorcycle);
        await _context.SaveChangesAsync();
        return motorcycle;
    }

    /// <summary>
    /// Search for a motorcycle by ID
    /// </summary>
    /// <param name="id">ID of the motorcycle</param>
    /// <returns>Found motorcycle or null if not found</returns>
    public async Task<Motorcycle?> GetByIdAsync(Guid id){
        return await _context.Motorcycles.FindAsync(id);
    }

    /// <summary>
    /// Search for all motorcycles
    /// </summary>
    /// <returns>List of all motorcycles</returns>
    public async Task<IReadOnlyList<Motorcycle>> GetAllAsync(){
        return await _context.Motorcycles.ToListAsync();
    }

    /// <summary>
    /// Search for a motorcycle by plate
    /// </summary>
    /// <param name="licensePlate">Plate of the motorcycle</param>
    /// <returns>Found motorcycle or null if not found</returns>
    public async Task<Motorcycle?> GetByPlateAsync(string plate){
        return await _context.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate);
    }

    /// <summary>
    /// Search for motorcycles by plate filter
    /// </summary>
    /// <param name="plateFilter">Plate filter (partial match)</param>
    /// <returns>List of motorcycles matching the filter</returns>
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

    /// <summary>
    /// Update an existing motorcycle
    /// </summary>
    /// <param name="motorcycle">Motorcycle with updated data</param>
    /// <returns>Updated motorcycle</returns>
    public async Task<Motorcycle> UpdateAsync(Motorcycle motorcycle){
        _context.Motorcycles.Update(motorcycle);
        await _context.SaveChangesAsync();
        return motorcycle;
    }

    /// <summary>
    /// Remove a motorcycle by ID
    /// </summary>
    /// <param name="id">ID of the motorcycle to be removed</param>
    /// <returns>True if removed successfully, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id){
        var motorcycle = await GetByIdAsync(id);
        if (motorcycle != null){
            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}