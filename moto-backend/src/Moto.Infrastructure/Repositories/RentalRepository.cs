// RentalRepository - implement repository for rentals
// implement IRentalRepository using Entity Framework

using Microsoft.EntityFrameworkCore;
using Moto.Domain.Interfaces;
using Moto.Domain.Entities;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

/// <summary>
/// RentalRepository - implement repository for rentals
/// implement IRentalRepository using Entity Framework
/// </summary>
public class RentalRepository : IRentalRepository{
   
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// RentalRepository - implement repository for rentals
    /// implement IRentalRepository using Entity Framework
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public RentalRepository(ApplicationDbContext context){
        _context = context;
    }

    /// <summary>
    /// Add a rental to the database
    /// </summary>
    /// <param name="rental">Rental to be added</param>
    /// <returns>Rental added with generated ID</returns>
    public async Task<Rental> AddAsync(Rental rental){
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    /// <summary>
    /// Search for a rental by ID
    /// </summary>
    /// <param name="id">ID of the rental</param>
    /// <returns>Found rental or null if not found</returns>
    public async Task<Rental?> GetByIdAsync(Guid id){
        return await _context.Rentals.FindAsync(id);
    }

    /// <summary>
    /// Search for all rentals
    /// </summary>
    /// <returns>List of all rentals</returns>
    public async Task<IEnumerable<Rental>> GetAllAsync(){
        return await _context.Rentals.ToListAsync();
    }
    
    /// <summary>
    /// Search for active rentals by motorcycle ID
    /// </summary>
    /// <param name="motorcycleId">ID of the motorcycle</param>
    /// <returns>List of active rentals</returns>
    public async Task<IEnumerable<Rental>> GetActiveRentalsByMotorcycleIdAsync(Guid motorcycleId){
        return await _context.Rentals.Where(r => r.MotorcycleId == motorcycleId && r.EndDate == null).ToListAsync();
    }

    /// <summary>
    /// Update a rental
    /// </summary>
    /// <param name="rental">Rental to be updated</param>
    /// <returns>Updated rental</returns>
    public async Task<Rental> UpdateAsync(Rental rental){
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
        return rental;
    }
}