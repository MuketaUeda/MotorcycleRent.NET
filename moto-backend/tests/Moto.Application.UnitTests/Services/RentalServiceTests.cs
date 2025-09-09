// RentalServiceTests - Unit tests for RentalService
// Tests business logic, validations and use cases
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Moto.Application.DTOs.Rentals;
using Moto.Application.Interfaces;
using Moto.Application.Services;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Domain.Enums;

namespace Moto.Application.UnitTests.Services;

public class RentalServiceTests
{
    private readonly Mock<IRentalRepository> _mockRentalRepository;
    private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
    private readonly Mock<ICourierRepository> _mockCourierRepository;
    private readonly Mock<IValidator<CreateRentalDto>> _mockCreateRentalValidator;
    private readonly Mock<IValidator<ReturnRentalDto>> _mockReturnRentalValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<RentalService>> _mockLogger;
    private readonly RentalService _service;

    public RentalServiceTests()
    {
        _mockRentalRepository = new Mock<IRentalRepository>();
        _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();
        _mockCourierRepository = new Mock<ICourierRepository>();
        _mockCreateRentalValidator = new Mock<IValidator<CreateRentalDto>>();
        _mockReturnRentalValidator = new Mock<IValidator<ReturnRentalDto>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<RentalService>>();

        _service = new RentalService(
            _mockRentalRepository.Object,
            _mockMotorcycleRepository.Object,
            _mockCourierRepository.Object,
            _mockCreateRentalValidator.Object,
            _mockReturnRentalValidator.Object,
            _mockMapper.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ShouldCreateRental()
    {
        // Arrange
        var request = new CreateRentalDto
        {
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(8),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = RentalPlan.SevenDays
        };

        var motorcycle = new Motorcycle
        {
            Id = "MOT001",
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var courier = new Courier
        {
            Id = "COU001",
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/cnh.png"
        };

        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            MotorcycleId = request.MotorcycleId,
            CourierId = request.CourierId,
            PlanType = request.PlanType,
            StartDate = request.StartDate,
            ExpectedEndDate = request.ExpectedEndDate
        };

        var rentalDto = new RentalDto
        {
            Id = rental.Id,
            MotorcycleId = request.MotorcycleId,
            CourierId = request.CourierId,
            PlanType = request.PlanType,
            StartDate = request.StartDate,
            ExpectedEndDate = request.ExpectedEndDate
        };

        var validationResult = new ValidationResult();
        _mockCreateRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.MotorcycleId))
            .ReturnsAsync(motorcycle);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.CourierId))
            .ReturnsAsync(courier);

        _mockRentalRepository.Setup(x => x.GetActiveRentalsByMotorcycleIdAsync(request.MotorcycleId))
            .ReturnsAsync(new List<Rental>());

        _mockRentalRepository.Setup(x => x.AddAsync(It.IsAny<Rental>()))
            .ReturnsAsync(rental);

        _mockMapper.Setup(x => x.Map<Rental>(request))
            .Returns(rental);

        _mockMapper.Setup(x => x.Map<RentalDto>(rental))
            .Returns(rentalDto);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.MotorcycleId.Should().Be(request.MotorcycleId);
        result.CourierId.Should().Be(request.CourierId);
        result.PlanType.Should().Be(request.PlanType);

        _mockRentalRepository.Verify(x => x.AddAsync(It.IsAny<Rental>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ValidationFails_ShouldThrowException()
    {
        // Arrange
        var request = new CreateRentalDto
        {
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(8),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = RentalPlan.SevenDays
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("StartDate", "Start date must be in the future")
        });

        _mockCreateRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<ValidationException>();

        _mockRentalRepository.Verify(x => x.AddAsync(It.IsAny<Rental>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_NonExistingMotorcycle_ShouldThrowException()
    {
        // Arrange
        var request = new CreateRentalDto
        {
            MotorcycleId = "NONEXISTENT",
            CourierId = "COU001",
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(8),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = RentalPlan.SevenDays
        };

        var validationResult = new ValidationResult();
        _mockCreateRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.MotorcycleId))
            .ReturnsAsync((Motorcycle?)null);

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*motorcycle not found*");

        _mockRentalRepository.Verify(x => x.AddAsync(It.IsAny<Rental>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_NonExistingCourier_ShouldThrowException()
    {
        // Arrange
        var request = new CreateRentalDto
        {
            MotorcycleId = "MOT001",
            CourierId = "NONEXISTENT",
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(8),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = RentalPlan.SevenDays
        };

        var motorcycle = new Motorcycle { Id = "MOT001" };

        var validationResult = new ValidationResult();
        _mockCreateRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.MotorcycleId))
            .ReturnsAsync(motorcycle);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.CourierId))
            .ReturnsAsync((Courier?)null);

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*courier not found*");

        _mockRentalRepository.Verify(x => x.AddAsync(It.IsAny<Rental>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_InvalidCnhType_ShouldThrowException()
    {
        // Arrange
        var request = new CreateRentalDto
        {
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(8),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = RentalPlan.SevenDays
        };

        var motorcycle = new Motorcycle { Id = "MOT001" };
        var courier = new Courier 
        { 
            Id = "COU001", 
            CnhType = CnhType.B // Invalid type for motorcycle rental
        };

        var validationResult = new ValidationResult();
        _mockCreateRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.MotorcycleId))
            .ReturnsAsync(motorcycle);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.CourierId))
            .ReturnsAsync(courier);

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*category A license*");

        _mockRentalRepository.Verify(x => x.AddAsync(It.IsAny<Rental>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_MotorcycleAlreadyRented_ShouldThrowException()
    {
        // Arrange
        var request = new CreateRentalDto
        {
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(8),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = RentalPlan.SevenDays
        };

        var motorcycle = new Motorcycle { Id = "MOT001" };
        var courier = new Courier { Id = "COU001", CnhType = CnhType.A };
        var activeRental = new Rental { Id = Guid.NewGuid(), MotorcycleId = "MOT001" };

        var validationResult = new ValidationResult();
        _mockCreateRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.MotorcycleId))
            .ReturnsAsync(motorcycle);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.CourierId))
            .ReturnsAsync(courier);

        _mockRentalRepository.Setup(x => x.GetActiveRentalsByMotorcycleIdAsync(request.MotorcycleId))
            .ReturnsAsync(new List<Rental> { activeRental });

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not available*");

        _mockRentalRepository.Verify(x => x.AddAsync(It.IsAny<Rental>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingRental_ShouldReturnRental()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var rental = new Rental
        {
            Id = rentalId,
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = DateTime.Now.AddDays(1),
            ExpectedEndDate = DateTime.Now.AddDays(8)
        };

        var rentalDto = new RentalDto
        {
            Id = rentalId,
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = DateTime.Now.AddDays(1),
            ExpectedEndDate = DateTime.Now.AddDays(8)
        };

        _mockRentalRepository.Setup(x => x.GetByIdAsync(rentalId))
            .ReturnsAsync(rental);

        _mockMapper.Setup(x => x.Map<RentalDto>(rental))
            .Returns(rentalDto);

        // Act
        var result = await _service.GetByIdAsync(rentalId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(rentalId);
        result.MotorcycleId.Should().Be("MOT001");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingRental_ShouldReturnNull()
    {
        // Arrange
        var rentalId = Guid.NewGuid();

        _mockRentalRepository.Setup(x => x.GetByIdAsync(rentalId))
            .ReturnsAsync((Rental?)null);

        // Act
        var result = await _service.GetByIdAsync(rentalId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ReturnAsync_ValidRequest_ShouldReturnRental()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var request = new ReturnRentalDto
        {
            ReturnDate = DateTime.Now
        };

        var rental = new Rental
        {
            Id = rentalId,
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = DateTime.Now.AddDays(-7),
            ExpectedEndDate = DateTime.Now,
            EndDate = null
        };

        var updatedRental = new Rental
        {
            Id = rentalId,
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = DateTime.Now.AddDays(-7),
            ExpectedEndDate = DateTime.Now,
            EndDate = request.ReturnDate
        };

        var rentalDto = new RentalDto
        {
            Id = rentalId,
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = DateTime.Now.AddDays(-7),
            ExpectedEndDate = DateTime.Now,
            EndDate = request.ReturnDate
        };

        var validationResult = new ValidationResult();
        _mockReturnRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockRentalRepository.Setup(x => x.GetByIdAsync(rentalId))
            .ReturnsAsync(rental);

        _mockRentalRepository.Setup(x => x.UpdateAsync(It.IsAny<Rental>()))
            .ReturnsAsync(updatedRental);

        _mockMapper.Setup(x => x.Map<RentalDto>(updatedRental))
            .Returns(rentalDto);

        // Act
        var result = await _service.ReturnAsync(rentalId, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(rentalId);
        result.EndDate.Should().Be(request.ReturnDate);

        _mockRentalRepository.Verify(x => x.UpdateAsync(It.IsAny<Rental>()), Times.Once);
    }

    [Fact]
    public async Task ReturnAsync_ValidationFails_ShouldThrowException()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var request = new ReturnRentalDto
        {
            ReturnDate = DateTime.Now
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("ReturnDate", "Return date must be after start date")
        });

        _mockReturnRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await _service.Invoking(x => x.ReturnAsync(rentalId, request))
            .Should().ThrowAsync<ValidationException>();

        _mockRentalRepository.Verify(x => x.UpdateAsync(It.IsAny<Rental>()), Times.Never);
    }

    [Fact]
    public async Task ReturnAsync_NonExistingRental_ShouldThrowException()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var request = new ReturnRentalDto
        {
            ReturnDate = DateTime.Now
        };

        var validationResult = new ValidationResult();
        _mockReturnRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockRentalRepository.Setup(x => x.GetByIdAsync(rentalId))
            .ReturnsAsync((Rental?)null);

        // Act & Assert
        await _service.Invoking(x => x.ReturnAsync(rentalId, request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not found*");

        _mockRentalRepository.Verify(x => x.UpdateAsync(It.IsAny<Rental>()), Times.Never);
    }

    [Fact]
    public async Task ReturnAsync_AlreadyReturnedRental_ShouldThrowException()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var request = new ReturnRentalDto
        {
            ReturnDate = DateTime.Now
        };

        var rental = new Rental
        {
            Id = rentalId,
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = DateTime.Now.AddDays(-7),
            ExpectedEndDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(-1) // Already returned
        };

        var validationResult = new ValidationResult();
        _mockReturnRentalValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockRentalRepository.Setup(x => x.GetByIdAsync(rentalId))
            .ReturnsAsync(rental);

        // Act & Assert
        await _service.Invoking(x => x.ReturnAsync(rentalId, request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already returned*");

        _mockRentalRepository.Verify(x => x.UpdateAsync(It.IsAny<Rental>()), Times.Never);
    }
}
