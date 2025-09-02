// MotorcycleService - Service for motorcycles
// Implements use cases: create, update, list, delete motorcycles
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Application.DTOs.Motorcycles;
using Moto.Application.DTOs.Events;
using Moto.Application.Interfaces;
using Moto.Application.Validators;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Moto.Application.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly IValidator<CreateMotorcycleDto> _createMotorcycleValidator;
    private readonly IValidator<UpdateMotorcycleDto> _updateMotorcycleValidator;
    private readonly IMapper _mapper;
    private readonly IEventPublisher? _eventPublisher;
    private readonly ILogger<MotorcycleService> _logger;

    // Dependency injection
    public MotorcycleService(
        IMotorcycleRepository motorcycleRepository, 
        IRentalRepository rentalRepository, 
        IValidator<CreateMotorcycleDto> createMotorcycleValidator,
        IValidator<UpdateMotorcycleDto> updateMotorcycleValidator,
        IMapper mapper, 
        ILogger<MotorcycleService> logger, 
        IEventPublisher? eventPublisher = null)
    {
        _motorcycleRepository = motorcycleRepository;
        _rentalRepository = rentalRepository;
        _createMotorcycleValidator = createMotorcycleValidator;
        _updateMotorcycleValidator = updateMotorcycleValidator;
        _mapper = mapper;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }
    
    // Create a motorcycle
    public async Task<MotorcycleDto> CreateAsync(CreateMotorcycleDto request)
    {
        _logger.LogInformation("Creating motorcycle with ID: {Id}, Plate: {Plate}", request.Id, request.Plate);
        
        // Validate input
        var validationResult = await _createMotorcycleValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Motorcycle creation failed - Validation errors: {Errors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }
        
        // Check if motorcycle with same ID already exists
        var existingMotorcycleById = await _motorcycleRepository.GetByIdAsync(request.Id);
        if (existingMotorcycleById != null)
        {
            _logger.LogWarning("Motorcycle creation failed - ID already exists: {Id}", request.Id);
            throw new InvalidOperationException("A motorcycle with this identifier already exists.");
        }

        var existingMotorcycle = await _motorcycleRepository.GetByPlateAsync(request.Plate);
        if (existingMotorcycle != null)
        {
            _logger.LogWarning("Motorcycle creation failed - Plate already exists: {Plate}", request.Plate);
            throw new InvalidOperationException("A motorcycle with this license plate already exists.");
        }

        var motorcycle = _mapper.Map<Motorcycle>(request);

        await _motorcycleRepository.AddAsync(motorcycle);
        _logger.LogInformation("Motorcycle created successfully: {Id}, {Plate}", motorcycle.Id, motorcycle.Plate);
        
        // Publish motorcycle created event
        var eventDto = _mapper.Map<MotorcycleCreatedEventDto>(motorcycle);
        _eventPublisher?.PublishMotorcycleCreatedEvent(eventDto);
        _logger.LogInformation("Motorcycle created event published: {Id}", motorcycle.Id);
        
        return _mapper.Map<MotorcycleDto>(motorcycle);
    }

    // Get a motorcycle by id
    public async Task<MotorcycleDto?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Getting motorcycle by ID: {Id}", id);
        
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null)
        {
            _logger.LogWarning("Motorcycle not found: {Id}", id);
            return null;
        }

        _logger.LogInformation("Motorcycle found: {Id}, {Plate}", motorcycle.Id, motorcycle.Plate);
        return _mapper.Map<MotorcycleDto>(motorcycle);
    }

    // Get all motorcycles with optional plate filter
    public async Task<IEnumerable<MotorcycleDto>> GetAllAsync(string? plateFilter = null)
    {
        var motorcycles = await _motorcycleRepository.GetByPlateFilterAsync(plateFilter);
        return _mapper.Map<IEnumerable<MotorcycleDto>>(motorcycles);
    }

    // Update a motorcycle
    public async Task<MotorcycleDto> UpdateAsync(string id, UpdateMotorcycleDto request)
    {
        _logger.LogInformation("Updating motorcycle: {Id}, New plate: {Plate}", id, request.Plate);
        
        // Validate input
        var validationResult = await _updateMotorcycleValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Motorcycle update failed - Validation errors: {Errors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }
        
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null)
        {
            _logger.LogWarning("Motorcycle not found for update: {Id}", id);
            throw new InvalidOperationException("Motorcycle not found.");
        }

        // Check if new plate already exists
        if (request.Plate != motorcycle.Plate)
        {
            var existingMotorcycle = await _motorcycleRepository.GetByPlateAsync(request.Plate);
            if (existingMotorcycle != null)
            {
                _logger.LogWarning("Motorcycle update failed - Plate already exists: {Plate}", request.Plate);
                throw new InvalidOperationException("A motorcycle with this license plate already exists.");
            }
        }

        // Update only the plate
        motorcycle.Plate = request.Plate;

        await _motorcycleRepository.UpdateAsync(motorcycle);
        _logger.LogInformation("Motorcycle updated successfully: {Id}, {Plate}", motorcycle.Id, motorcycle.Plate);

        return _mapper.Map<MotorcycleDto>(motorcycle);
    }

    // Delete a motorcycle
    public async Task<bool> DeleteAsync(string id)
    {
        _logger.LogInformation("Deleting motorcycle: {Id}", id);
        
        var motorcycleResult = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycleResult == null)
        {
            _logger.LogWarning("Motorcycle not found for deletion: {Id}", id);
            throw new InvalidOperationException("Motorcycle not found.");
        }
        
        var activeRentals = await _rentalRepository.GetActiveRentalsByMotorcycleIdAsync(id);
        if (activeRentals.Any())
        {
            _logger.LogWarning("Cannot delete motorcycle with active rentals: {Id}", id);
            throw new InvalidOperationException("Cannot delete motorcycle with active rentals.");
        }
        
        var result = await _motorcycleRepository.DeleteAsync(id);
        _logger.LogInformation("Motorcycle deleted successfully: {Id}", id);
        
        return result;
    }
}
