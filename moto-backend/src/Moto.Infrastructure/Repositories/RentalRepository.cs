// RentalRepository - implement repository for rentals
// implement IRentalRepository using Entity Framework

using Microsoft.EntityFrameworkCore;
using Moto.Domain.Interfaces;
using Moto.Domain.Entities;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

public class RentalRepository : IRentalRepository{
   
    private readonly ApplicationDbContext _context;


    /// RentalRepository - implement repository for rentals
    /// implement IRentalRepository using Entity Framework

    public RentalRepository(ApplicationDbContext context){
        _context = context;
    }

   
    /// Add a rental to the database
    public async Task<Rental> AddAsync(Rental rental){
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    /// Search for a rental by ID
    public async Task<Rental?> GetByIdAsync(Guid id){
        return await _context.Rentals.FindAsync(id);
    }
    
    /// Search for active rentals by motorcycle ID
    public async Task<IEnumerable<Rental>> GetActiveRentalsByMotorcycleIdAsync(string motorcycleId){
        return await _context.Rentals.Where(r => r.MotorcycleId == motorcycleId && r.EndDate == null).ToListAsync();
    }

    /// Update a rental

    public async Task<Rental> UpdateAsync(Rental rental){
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
        return rental;
    }
}