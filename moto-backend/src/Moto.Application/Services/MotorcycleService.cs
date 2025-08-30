// MotorcycleService - Service for motorcycles
// Implements use cases: create, update, list, delete motorcycles
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Application.DTOs.Motorcycles;
using Moto.Application.Interfaces;
using AutoMapper;

namespace Moto.Application.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly IMapper _mapper;

    // Dependency injection
    public MotorcycleService(IMotorcycleRepository motorcycleRepository, IRentalRepository rentalRepository, IMapper mapper)
    {
        _motorcycleRepository = motorcycleRepository;
        _rentalRepository = rentalRepository;
        _mapper = mapper;
    }
    
    // Create a motorcycle
    public async Task<MotorcycleDto> CreateAsync(CreateMotorcycleDto request)
    {
        var existingMotorcycle = await _motorcycleRepository.GetByPlateAsync(request.Plate);
        if (existingMotorcycle != null)
        {
            throw new InvalidOperationException("A motorcycle with this license plate already exists.");
        }

        var motorcycle = _mapper.Map<Motorcycle>(request);
        motorcycle.Id = Guid.NewGuid();

        await _motorcycleRepository.AddAsync(motorcycle);
        
        return _mapper.Map<MotorcycleDto>(motorcycle);
    }

    // Get a motorcycle by id
    public async Task<MotorcycleDto?> GetByIdAsync(Guid id)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null)
        {
            return null;
        }

        return _mapper.Map<MotorcycleDto>(motorcycle);
    }

    // Get all motorcycles with optional plate filter
    public async Task<IEnumerable<MotorcycleDto>> GetAllAsync(string? plateFilter = null)
    {
        var motorcycles = await _motorcycleRepository.GetByPlateFilterAsync(plateFilter);
        return _mapper.Map<IEnumerable<MotorcycleDto>>(motorcycles);
    }

    // Update a motorcycle
    public async Task<MotorcycleDto> UpdateAsync(Guid id, UpdateMotorcycleDto request)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null)
        {
            throw new InvalidOperationException("Motorcycle not found.");
        }

        // Check if new plate already exists (if plate is being updated)
        if (!string.IsNullOrEmpty(request.Plate) && request.Plate != motorcycle.Plate)
        {
            var existingMotorcycle = await _motorcycleRepository.GetByPlateAsync(request.Plate);
            if (existingMotorcycle != null)
            {
                throw new InvalidOperationException("A motorcycle with this license plate already exists.");
            }
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Model))
        {
            motorcycle.Model = request.Model;
        }

        if (!string.IsNullOrEmpty(request.Plate))
        {
            motorcycle.Plate = request.Plate;
        }

        if (request.Year.HasValue)
        {
            motorcycle.Year = request.Year.Value;
        }

        await _motorcycleRepository.UpdateAsync(motorcycle);

        return _mapper.Map<MotorcycleDto>(motorcycle);
    }

    // Delete a motorcycle
    public async Task<bool> DeleteAsync(Guid id)
    {
        var motorcycleResult = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycleResult == null)
        {
            throw new InvalidOperationException("Motorcycle not found.");
        }
        
        var activeRentals = await _rentalRepository.GetActiveRentalsByMotorcycleIdAsync(id);
        if (activeRentals.Any())
        {
            throw new InvalidOperationException("Cannot delete motorcycle with active rentals.");
        }
        
        return await _motorcycleRepository.DeleteAsync(id);
    }
}
