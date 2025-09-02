// CourierService - Application service for couriers
// Implements use cases: create courier and update CNH image
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Application.DTOs.Couriers;
using Moto.Application.Interfaces;
using Moto.Application.Validators;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Moto.Application.Services;

public class CourierService : ICourierService
{
    private readonly ICourierRepository _courierRepository;
    private readonly IValidator<CreateCourierDto> _createCourierValidator;
    private readonly IValidator<UpdateCnhImageDto> _updateCnhImageValidator;
    private readonly IMapper _mapper;
    private readonly ILogger<CourierService> _logger;

    // Dependency injection
    public CourierService(
        ICourierRepository courierRepository, 
        IValidator<CreateCourierDto> createCourierValidator,
        IValidator<UpdateCnhImageDto> updateCnhImageValidator,
        IMapper mapper, 
        ILogger<CourierService> logger)
    {
        _courierRepository = courierRepository;
        _createCourierValidator = createCourierValidator;
        _updateCnhImageValidator = updateCnhImageValidator;
        _mapper = mapper;
        _logger = logger;
    }
    
    // Create a courier
    public async Task<CourierDto> CreateAsync(CreateCourierDto request)
    {
        _logger.LogInformation("Creating courier with ID: {Id}, CNPJ: {Cnpj}", request.Id, request.Cnpj);
        
        // Validate input
        var validationResult = await _createCourierValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Courier creation failed - Validation errors: {Errors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }
        
        // Check if a courier with the same ID already exists
        var existingCourierById = await _courierRepository.GetByIdAsync(request.Id);
        if (existingCourierById != null)
        {
            _logger.LogWarning("Courier creation failed - ID already exists: {Id}", request.Id);
            throw new InvalidOperationException("A courier with this ID already exists.");
        }

        // Check if a courier with the same CNPJ already exists
        var existingCourierByCnpj = await _courierRepository.GetByCnpjAsync(request.Cnpj);
        if (existingCourierByCnpj != null)
        {
            _logger.LogWarning("Courier creation failed - CNPJ already exists: {Cnpj}", request.Cnpj);
            throw new InvalidOperationException("A courier with this CNPJ already exists.");
        }

        // Check if a courier with the same CNH number already exists
        var existingCourierByCnh = await _courierRepository.GetByCnhNumberAsync(request.CnhNumber);
        if (existingCourierByCnh != null)
        {
            _logger.LogWarning("Courier creation failed - CNH already exists: {CnhNumber}", request.CnhNumber);
            throw new InvalidOperationException("A courier with this CNH number already exists.");
        }

        var courier = _mapper.Map<Courier>(request);

        await _courierRepository.AddAsync(courier);
        _logger.LogInformation("Courier created successfully: {Id}, {Name}", courier.Id, courier.Name);
        
        return _mapper.Map<CourierDto>(courier);
    }

    // Update CNH image
    public async Task<CourierDto> UpdateCnhImageAsync(string id, UpdateCnhImageDto request)
    {
        _logger.LogInformation("Updating CNH image for courier: {Id}", id);
        
        // Validate input
        var validationResult = await _updateCnhImageValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("CNH image update failed - Validation errors: {Errors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }
        
        var courier = await _courierRepository.GetByIdAsync(id);
        if (courier == null)
        {
            _logger.LogWarning("Courier not found for CNH image update: {Id}", id);
            throw new InvalidOperationException("Courier not found.");
        }
        // Update only the CNH image
        courier.CnhImageUrl = request.CnhImageUrl;

        await _courierRepository.UpdateAsync(courier);
        _logger.LogInformation("CNH image updated successfully for courier: {Id}", id);

        return _mapper.Map<CourierDto>(courier);
    }
}