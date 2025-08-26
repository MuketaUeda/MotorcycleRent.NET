// MotorcycleService - Service for motorcycles
// Implements use cases: create, update, list, delete motorcycles
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Application.DTOs;

namespace Moto.Application.Services;

public class MotorcycleService{
    private readonly IMotorcycleRepository _motorcycleRepository;

    // Dependency injection
    public MotorcycleService(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }
    
    // Create a motorcycle
    public async Task<MotorcycleResponse> CreateAsync(CreateMotorcycleRequest request){
        var existingMotorcycle = await _motorcycleRepository.GetByPlateAsync(request.LicensePlate);
        if (existingMotorcycle != null)
        {
            throw new InvalidOperationException("A motorcycle with this license plate already exists.");
        }

        var motorcycle = new Motorcycle{
            Id = Guid.NewGuid(),
            Year = request.Year,
            Model = request.Model,
            Plate = request.LicensePlate,
            CreatedAt = DateTime.UtcNow,
        };

        await _motorcycleRepository.AddAsync(motorcycle);
        
        return new MotorcycleResponse
        {
            Id = motorcycle.Id,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate,
            Year = motorcycle.Year
        };
    }

    // Get a motorcycle by id
    public async Task<MotorcycleResponse?> GetByIdAsync(Guid id){
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null)
        {
            return null;
        }

        return new MotorcycleResponse
        {
            Id = motorcycle.Id,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate,
            Year = motorcycle.Year
        };
    }

    // Get all motorcycles
    public async Task<IEnumerable<MotorcycleResponse>> GetAllAsync(){
        var motorcycles = await _motorcycleRepository.GetAllAsync();
        
        return motorcycles.Select(motorcycle => new MotorcycleResponse
        {
            Id = motorcycle.Id,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate,
            Year = motorcycle.Year
        });
    }
}
