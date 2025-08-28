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

    /// Constructor for RentalService
    public RentalService(
        IRentalRepository rentalRepository,
        IMotorcycleRepository motorcycleRepository,
        ICourierRepository courierRepository,
        IValidator<CreateRentalDto> createRentalValidator,
        IValidator<ReturnRentalDto> returnRentalValidator,
        IMapper mapper
    )
    {
        _rentalRepository = rentalRepository;
        _motorcycleRepository = motorcycleRepository;
        _courierRepository = courierRepository;
        _createRentalValidator = createRentalValidator;
        _returnRentalValidator = returnRentalValidator;
        _mapper = mapper;
    }
}
