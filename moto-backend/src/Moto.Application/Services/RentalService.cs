// RentalService - Service for rentals
// Implement use cases: create rental, return rental, calculate values
using Moto.Domain.Interfaces;
using Moto.Domain.Entities;
using Moto.Domain.Enums;
using Moto.Application.Interfaces;
using Moto.Application.Validators;
using Moto.Application.DTOs.Rentals;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Moto.Application.Services;

/// RentalService - Service for rentals
public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly ICourierRepository _courierRepository;
    private readonly IValidator<CreateRentalDto> _createRentalValidator;
    private readonly IValidator<ReturnRentalDto> _returnRentalValidator;
    private readonly IMapper _mapper;
    private readonly ILogger<RentalService> _logger;

    /// Constructor for RentalService
    public RentalService(
        IRentalRepository rentalRepository,
        IMotorcycleRepository motorcycleRepository,
        ICourierRepository courierRepository,
        IValidator<CreateRentalDto> createRentalValidator,
        IValidator<ReturnRentalDto> returnRentalValidator,
        IMapper mapper,
        ILogger<RentalService> logger
    )
    {
        _rentalRepository = rentalRepository;
        _motorcycleRepository = motorcycleRepository;
        _courierRepository = courierRepository;
        _createRentalValidator = createRentalValidator;
        _returnRentalValidator = returnRentalValidator;
        _mapper = mapper;
        _logger = logger;
    }

    /// Creates a new rental
    public async Task<RentalDto> CreateAsync(CreateRentalDto createRentalDto)
    {
        _logger.LogInformation("Creating rental - Motorcycle: {MotorcycleId}, Courier: {CourierId}", 
            createRentalDto.MotorcycleId, createRentalDto.CourierId);
        
        // Validate input
        var validationResult = await _createRentalValidator.ValidateAsync(createRentalDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Rental creation failed - Validation errors: {Errors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        // Check if motorcycle exists
        var motorcycle = await _motorcycleRepository.GetByIdAsync(createRentalDto.MotorcycleId);
        if (motorcycle == null)
        {
            _logger.LogWarning("Rental creation failed - Motorcycle not found: {MotorcycleId}", createRentalDto.MotorcycleId);
            throw new InvalidOperationException("Motorcycle not found.");
        }

        // Check if courier exists
        var courier = await _courierRepository.GetByIdAsync(createRentalDto.CourierId);
        if (courier == null)
        {
            _logger.LogWarning("Rental creation failed - Courier not found: {CourierId}", createRentalDto.CourierId);
            throw new InvalidOperationException("Courier not found.");
        }

        // Business rule: Only couriers with category A can rent motorcycles
        if (courier.CnhType != CnhType.A && courier.CnhType != CnhType.AB)
        {
            _logger.LogWarning("Rental creation failed - Invalid CNH type: {CourierId}, Type: {CnhType}", 
                createRentalDto.CourierId, courier.CnhType);
            throw new InvalidOperationException("Only couriers with category A license can rent motorcycles.");
        }

        // Check if motorcycle is available (no active rentals)
        var activeRentals = await _rentalRepository.GetActiveRentalsByMotorcycleIdAsync(createRentalDto.MotorcycleId);
        if (activeRentals.Any())
        {
            _logger.LogWarning("Rental creation failed - Motorcycle not available: {MotorcycleId}", createRentalDto.MotorcycleId);
            throw new InvalidOperationException("Motorcycle is not available for rental.");
        }

        // Calculate dates according to business rules
        var startDate = createRentalDto.StartDate;
        var expectedEndDate = createRentalDto.ExpectedEndDate;

        // Create rental entity
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            MotorcycleId = createRentalDto.MotorcycleId,
            CourierId = createRentalDto.CourierId,
            PlanType = createRentalDto.PlanType,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            EndDate = null, // Will be set when rental is returned
            TotalCost = null,
            FineAmount = null,
            AdditionalDaysCost = null,
            AdditionalDays = null
        };

        // Save to database
        var createdRental = await _rentalRepository.AddAsync(rental);
        _logger.LogInformation("Rental created successfully: {RentalId}, Motorcycle: {MotorcycleId}, Courier: {CourierId}", 
            createdRental.Id, createdRental.MotorcycleId, createdRental.CourierId);

        // Map to DTO and return
        return _mapper.Map<RentalDto>(createdRental);
    }

    /// Gets a rental by ID
    public async Task<RentalDto?> GetByIdAsync(Guid id)
    {
        var rental = await _rentalRepository.GetByIdAsync(id);
        if (rental == null)
        {
            return null;
        }

        return _mapper.Map<RentalDto>(rental);
    }

    /// Finalizes a rental (return)
    public async Task<RentalDto> ReturnAsync(Guid id, ReturnRentalDto returnRentalDto)
    {
        // Validate input
        var validationResult = await _returnRentalValidator.ValidateAsync(returnRentalDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Get the rental
        var rental = await _rentalRepository.GetByIdAsync(id);
        if (rental == null)
        {
            throw new InvalidOperationException("Rental not found.");
        }

        // Check if the rental is already returned
        if (rental.EndDate.HasValue)
        {
            throw new InvalidOperationException("Rental already returned");
        }

        // Set the return date
        rental.EndDate = returnRentalDto.ReturnDate;

        // Calculate costs based on actual rental period (from start date to return date)
        var actualRentalDays = (returnRentalDto.ReturnDate - rental.StartDate).Days;
        var baseCost = actualRentalDays * GetDailyCost(rental.PlanType);
        
        var additionalDays = 0;
        var additionalDaysCost = 0m;
        var fineAmount = 0m;

        if (returnRentalDto.ReturnDate < rental.ExpectedEndDate)
        {
            // Early return - calculate fine on unused days
            var unusedDays = (rental.ExpectedEndDate - returnRentalDto.ReturnDate).Days;
            var unusedDaysCost = unusedDays * GetDailyCost(rental.PlanType);

            // Calculate fine based on plan type
            fineAmount = rental.PlanType switch
            {
                RentalPlan.SevenDays => unusedDaysCost * 0.20m, // 20% fine
                RentalPlan.FifteenDays => unusedDaysCost * 0.40m, // 40% fine
                _ => 0m // No fine for other plans
            };
        }
        else if (returnRentalDto.ReturnDate > rental.ExpectedEndDate)
        {
            // Late return - calculate additional days cost
            additionalDays = (returnRentalDto.ReturnDate - rental.ExpectedEndDate).Days;
            additionalDaysCost = additionalDays * 50.00m; // R$50,00 per additional day
        }

        // Calculate total cost: base cost + additional days + fine
        var totalCost = baseCost + additionalDaysCost + fineAmount;

        // Update rental with calculated values
        rental.AdditionalDays = additionalDays;
        rental.AdditionalDaysCost = additionalDaysCost;
        rental.FineAmount = fineAmount;
        rental.TotalCost = totalCost;

        // Save the updated rental
        var updatedRental = await _rentalRepository.UpdateAsync(rental);

        // Return the rental
        return _mapper.Map<RentalDto>(updatedRental);
    }

    /// Gets daily rate for rental plan
    private decimal GetDailyRate(RentalPlan planType)
    {
        return planType switch
        {
            RentalPlan.SevenDays => 30.00m,
            RentalPlan.FifteenDays => 28.00m,
            RentalPlan.ThirtyDays => 22.00m,
            RentalPlan.FortyFiveDays => 20.00m,
            RentalPlan.FiftyDays => 18.00m,
            _ => throw new ArgumentException("Invalid rental plan type.")
        };
    }

    /// Gets fine percentage for early return
    private decimal GetFinePercentage(RentalPlan planType)
    {
        return planType switch
        {
            RentalPlan.SevenDays => 0.20m,  // 20%
            RentalPlan.FifteenDays => 0.40m, // 40%
            _ => 0.00m // No fine for other plans
        };
    }

    /// Get the cost of the plan
    private decimal GetPlanCost(RentalPlan planType)
    {
        return planType switch
        {
            RentalPlan.SevenDays => 30.00m * 7,
            RentalPlan.FifteenDays => 28.00m * 15,
            RentalPlan.ThirtyDays => 22.00m * 30,
            RentalPlan.FortyFiveDays => 20.00m * 45,
            RentalPlan.FiftyDays => 18.00m * 50,
            _ => throw new ArgumentException("Invalid plan type")
        };
    }

    /// Get the daily cost for a plan
    private decimal GetDailyCost(RentalPlan planType)
    {
        return planType switch
        {
            RentalPlan.SevenDays => 30.00m,
            RentalPlan.FifteenDays => 28.00m,
            RentalPlan.ThirtyDays => 22.00m,
            RentalPlan.FortyFiveDays => 20.00m,
            RentalPlan.FiftyDays => 18.00m,
            _ => throw new ArgumentException("Invalid plan type")
        };
    }
}
