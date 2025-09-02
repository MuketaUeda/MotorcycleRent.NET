// MotorcycleServiceTests - Unit tests for MotorcycleService
// Tests business logic, validations and use cases
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Moto.Application.DTOs.Motorcycles;
using Moto.Application.DTOs.Events;
using Moto.Application.Interfaces;
using Moto.Application.Services;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Domain.Enums;

namespace Moto.Application.UnitTests.Services;

public class MotorcycleServiceTests
{
    private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
    private readonly Mock<IRentalRepository> _mockRentalRepository;
    private readonly Mock<IValidator<CreateMotorcycleDto>> _mockCreateMotorcycleValidator;
    private readonly Mock<IValidator<UpdateMotorcycleDto>> _mockUpdateMotorcycleValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<MotorcycleService>> _mockLogger;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly MotorcycleService _service;

    public MotorcycleServiceTests()
    {
        _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();
        _mockRentalRepository = new Mock<IRentalRepository>();
        _mockCreateMotorcycleValidator = new Mock<IValidator<CreateMotorcycleDto>>();
        _mockUpdateMotorcycleValidator = new Mock<IValidator<UpdateMotorcycleDto>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<MotorcycleService>>();
        _mockEventPublisher = new Mock<IEventPublisher>();

        _service = new MotorcycleService(
            _mockMotorcycleRepository.Object,
            _mockRentalRepository.Object,
            _mockCreateMotorcycleValidator.Object,
            _mockUpdateMotorcycleValidator.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockEventPublisher.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ShouldCreateMotorcycle()
    {
        // Arrange
        var request = new CreateMotorcycleDto
        {
            Id = "MOT001",
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var motorcycle = new Motorcycle
        {
            Id = request.Id,
            Plate = request.Plate,
            Year = request.Year,
            Model = request.Model
        };

        var motorcycleDto = new MotorcycleDto
        {
            Id = request.Id,
            Plate = request.Plate,
            Year = request.Year,
            Model = request.Model
        };

        var eventDto = new MotorcycleCreatedEventDto
        {
            Id = request.Id,
            Plate = request.Plate,
            Year = request.Year,
            Model = request.Model
        };

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync((Motorcycle?)null);

        _mockMotorcycleRepository.Setup(x => x.GetByPlateAsync(request.Plate))
            .ReturnsAsync((Motorcycle?)null);

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockCreateMotorcycleValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMapper.Setup(x => x.Map<Motorcycle>(request))
            .Returns(motorcycle);

        _mockMotorcycleRepository.Setup(x => x.AddAsync(motorcycle))
            .ReturnsAsync(motorcycle);

        _mockMapper.Setup(x => x.Map<MotorcycleCreatedEventDto>(motorcycle))
            .Returns(eventDto);

        _mockMapper.Setup(x => x.Map<MotorcycleDto>(motorcycle))
            .Returns(motorcycleDto);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(request.Id);
        result.Plate.Should().Be(request.Plate);
        result.Year.Should().Be(request.Year);
        result.Model.Should().Be(request.Model);

        _mockMotorcycleRepository.Verify(x => x.AddAsync(motorcycle), Times.Once);
        _mockEventPublisher.Verify(x => x.PublishMotorcycleCreatedEvent(eventDto), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DuplicateId_ShouldThrowException()
    {
        // Arrange
        var request = new CreateMotorcycleDto
        {
            Id = "MOT001",
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var existingMotorcycle = new Motorcycle
        {
            Id = request.Id,
            Plate = "XYZ5678",
            Year = 2023,
            Model = "Yamaha YBR 125"
        };

        // Setup validator to return success (validation passes)
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockCreateMotorcycleValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync(existingMotorcycle);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(request));

        exception.Message.Should().Be("A motorcycle with this identifier already exists.");
        _mockMotorcycleRepository.Verify(x => x.AddAsync(It.IsAny<Motorcycle>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_DuplicatePlate_ShouldThrowException()
    {
        // Arrange
        var request = new CreateMotorcycleDto
        {
            Id = "MOT001",
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var existingMotorcycle = new Motorcycle
        {
            Id = "MOT002",
            Plate = request.Plate,
            Year = 2023,
            Model = "Yamaha YBR 125"
        };

        // Setup validator to return success (validation passes)
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockCreateMotorcycleValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync((Motorcycle?)null);

        _mockMotorcycleRepository.Setup(x => x.GetByPlateAsync(request.Plate))
            .ReturnsAsync(existingMotorcycle);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(request));

        exception.Message.Should().Be("A motorcycle with this license plate already exists.");
        _mockMotorcycleRepository.Verify(x => x.AddAsync(It.IsAny<Motorcycle>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnMotorcycle()
    {
        // Arrange
        var id = "MOT001";
        var motorcycle = new Motorcycle
        {
            Id = id,
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var motorcycleDto = new MotorcycleDto
        {
            Id = id,
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(motorcycle);

        _mockMapper.Setup(x => x.Map<MotorcycleDto>(motorcycle))
            .Returns(motorcycleDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Plate.Should().Be("ABC1234");
        result.Year.Should().Be(2024);
        result.Model.Should().Be("Honda CG 160");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        var id = "MOT999";

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Motorcycle?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_WithPlateFilter_ShouldReturnFilteredMotorcycles()
    {
        // Arrange
        var plateFilter = "ABC";
        var motorcycles = new List<Motorcycle>
        {
            new() { Id = "MOT001", Plate = "ABC1234", Year = 2024, Model = "Honda CG 160" },
            new() { Id = "MOT002", Plate = "ABC5678", Year = 2023, Model = "Yamaha YBR 125" }
        };

        var motorcycleDtos = new List<MotorcycleDto>
        {
            new() { Id = "MOT001", Plate = "ABC1234", Year = 2024, Model = "Honda CG 160" },
            new() { Id = "MOT002", Plate = "ABC5678", Year = 2023, Model = "Yamaha YBR 125" }
        };

        _mockMotorcycleRepository.Setup(x => x.GetByPlateFilterAsync(plateFilter))
            .ReturnsAsync(motorcycles);

        _mockMapper.Setup(x => x.Map<IEnumerable<MotorcycleDto>>(motorcycles))
            .Returns(motorcycleDtos);

        // Act
        var result = await _service.GetAllAsync(plateFilter);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(m => m.Plate.Should().Contain("ABC"));
    }

    [Fact]
    public async Task GetAllAsync_WithoutPlateFilter_ShouldReturnAllMotorcycles()
    {
        // Arrange
        var motorcycles = new List<Motorcycle>
        {
            new() { Id = "MOT001", Plate = "ABC1234", Year = 2024, Model = "Honda CG 160" },
            new() { Id = "MOT002", Plate = "XYZ5678", Year = 2023, Model = "Yamaha YBR 125" }
        };

        var motorcycleDtos = new List<MotorcycleDto>
        {
            new() { Id = "MOT001", Plate = "ABC1234", Year = 2024, Model = "Honda CG 160" },
            new() { Id = "MOT002", Plate = "XYZ5678", Year = 2023, Model = "Yamaha YBR 125" }
        };

        _mockMotorcycleRepository.Setup(x => x.GetByPlateFilterAsync(null))
            .ReturnsAsync(motorcycles);

        _mockMapper.Setup(x => x.Map<IEnumerable<MotorcycleDto>>(motorcycles))
            .Returns(motorcycleDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ValidRequest_ShouldUpdateMotorcycle()
    {
        // Arrange
        var id = "MOT001";
        var request = new UpdateMotorcycleDto
        {
            Plate = "XYZ5678"
        };

        var existingMotorcycle = new Motorcycle
        {
            Id = id,
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var updatedMotorcycle = new Motorcycle
        {
            Id = id,
            Plate = request.Plate,
            Year = 2024,
            Model = "Honda CG 160"
        };

        var motorcycleDto = new MotorcycleDto
        {
            Id = id,
            Plate = request.Plate,
            Year = 2024,
            Model = "Honda CG 160"
        };

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateMotorcycleValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(existingMotorcycle);

        _mockMotorcycleRepository.Setup(x => x.GetByPlateAsync(request.Plate))
            .ReturnsAsync((Motorcycle?)null);

        _mockMotorcycleRepository.Setup(x => x.UpdateAsync(It.IsAny<Motorcycle>()))
            .ReturnsAsync(existingMotorcycle);

        _mockMapper.Setup(x => x.Map<MotorcycleDto>(It.IsAny<Motorcycle>()))
            .Returns(motorcycleDto);

        // Act
        var result = await _service.UpdateAsync(id, request);

        // Assert
        result.Should().NotBeNull();
        result.Plate.Should().Be(request.Plate);
        _mockMotorcycleRepository.Verify(x => x.UpdateAsync(It.IsAny<Motorcycle>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = "MOT999";
        var request = new UpdateMotorcycleDto
        {
            Plate = "XYZ5678"
        };

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateMotorcycleValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Motorcycle?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(id, request));

        exception.Message.Should().Be("Motorcycle not found.");
        _mockMotorcycleRepository.Verify(x => x.UpdateAsync(It.IsAny<Motorcycle>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_DuplicatePlate_ShouldThrowException()
    {
        // Arrange
        var id = "MOT001";
        var request = new UpdateMotorcycleDto
        {
            Plate = "XYZ5678"
        };

        var existingMotorcycle = new Motorcycle
        {
            Id = id,
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var duplicateMotorcycle = new Motorcycle
        {
            Id = "MOT002",
            Plate = request.Plate,
            Year = 2023,
            Model = "Yamaha YBR 125"
        };

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateMotorcycleValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(existingMotorcycle);

        _mockMotorcycleRepository.Setup(x => x.GetByPlateAsync(request.Plate))
            .ReturnsAsync(duplicateMotorcycle);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(id, request));

        exception.Message.Should().Be("A motorcycle with this license plate already exists.");
        _mockMotorcycleRepository.Verify(x => x.UpdateAsync(It.IsAny<Motorcycle>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ShouldDeleteMotorcycle()
    {
        // Arrange
        var id = "MOT001";
        var motorcycle = new Motorcycle
        {
            Id = id,
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(motorcycle);

        _mockRentalRepository.Setup(x => x.GetActiveRentalsByMotorcycleIdAsync(id))
            .ReturnsAsync(new List<Domain.Entities.Rental>());

        _mockMotorcycleRepository.Setup(x => x.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
        _mockMotorcycleRepository.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = "MOT999";

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Motorcycle?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.DeleteAsync(id));

        exception.Message.Should().Be("Motorcycle not found.");
        _mockMotorcycleRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithActiveRentals_ShouldThrowException()
    {
        // Arrange
        var id = "MOT001";
        var motorcycle = new Motorcycle
        {
            Id = id,
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        var activeRentals = new List<Domain.Entities.Rental>
        {
            new() { Id = Guid.NewGuid(), MotorcycleId = id }
        };

        _mockMotorcycleRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(motorcycle);

        _mockRentalRepository.Setup(x => x.GetActiveRentalsByMotorcycleIdAsync(id))
            .ReturnsAsync(activeRentals);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.DeleteAsync(id));

        exception.Message.Should().Be("Cannot delete motorcycle with active rentals.");
        _mockMotorcycleRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }
}
