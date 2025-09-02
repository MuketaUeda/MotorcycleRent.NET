// CourierServiceTests - Unit tests for CourierService
// Tests business logic, validations and use cases
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Moto.Application.DTOs.Couriers;
using Moto.Application.Interfaces;
using Moto.Application.Services;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Domain.Enums;

namespace Moto.Application.UnitTests.Services;

public class CourierServiceTests
{
    private readonly Mock<ICourierRepository> _mockCourierRepository;
    private readonly Mock<IValidator<CreateCourierDto>> _mockCreateCourierValidator;
    private readonly Mock<IValidator<UpdateCnhImageDto>> _mockUpdateCnhImageValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CourierService>> _mockLogger;
    private readonly CourierService _service;

    public CourierServiceTests()
    {
        _mockCourierRepository = new Mock<ICourierRepository>();
        _mockCreateCourierValidator = new Mock<IValidator<CreateCourierDto>>();
        _mockUpdateCnhImageValidator = new Mock<IValidator<UpdateCnhImageDto>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CourierService>>();

        _service = new CourierService(
            _mockCourierRepository.Object,
            _mockCreateCourierValidator.Object,
            _mockUpdateCnhImageValidator.Object,
            _mockMapper.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ShouldCreateCourier()
    {
        // Arrange
        var request = new CreateCourierDto
        {
            Id = "COU001",
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/cnh.png"
        };

        var courier = new Courier
        {
            Id = request.Id,
            Cnpj = request.Cnpj,
            Name = request.Name,
            BirthDate = request.BirthDate,
            CnhNumber = request.CnhNumber,
            CnhType = request.CnhType,
            CnhImageUrl = request.CnhImageUrl
        };

        var courierDto = new CourierDto
        {
            Id = request.Id,
            Cnpj = request.Cnpj,
            Name = request.Name,
            BirthDate = request.BirthDate,
            CnhNumber = request.CnhNumber,
            CnhType = request.CnhType,
            CnhImageUrl = request.CnhImageUrl
        };

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync((Courier?)null);

        _mockCourierRepository.Setup(x => x.GetByCnpjAsync(request.Cnpj))
            .ReturnsAsync((Courier?)null);

        _mockCourierRepository.Setup(x => x.GetByCnhNumberAsync(request.CnhNumber))
            .ReturnsAsync((Courier?)null);

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockCreateCourierValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockMapper.Setup(x => x.Map<Courier>(request))
            .Returns(courier);

        _mockCourierRepository.Setup(x => x.AddAsync(courier))
            .ReturnsAsync(courier);

        _mockMapper.Setup(x => x.Map<CourierDto>(courier))
            .Returns(courierDto);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(request.Id);
        result.Cnpj.Should().Be(request.Cnpj);
        result.Name.Should().Be(request.Name);
        result.CnhNumber.Should().Be(request.CnhNumber);
        result.CnhType.Should().Be(request.CnhType);

        _mockCourierRepository.Verify(x => x.AddAsync(courier), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DuplicateId_ShouldThrowException()
    {
        // Arrange
        var request = new CreateCourierDto
        {
            Id = "COU001",
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/cnh.png"
        };

        var existingCourier = new Courier { Id = request.Id };

        // Setup validator to return success (validation passes)
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockCreateCourierValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync(existingCourier);

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");

        _mockCourierRepository.Verify(x => x.AddAsync(It.IsAny<Courier>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_DuplicateCnpj_ShouldThrowException()
    {
        // Arrange
        var request = new CreateCourierDto
        {
            Id = "COU001",
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/cnh.png"
        };

        var existingCourier = new Courier { Cnpj = request.Cnpj };

        // Setup validator to return success (validation passes)
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockCreateCourierValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync((Courier?)null);

        _mockCourierRepository.Setup(x => x.GetByCnpjAsync(request.Cnpj))
            .ReturnsAsync(existingCourier);

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*CNPJ already exists*");

        _mockCourierRepository.Verify(x => x.AddAsync(It.IsAny<Courier>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_DuplicateCnhNumber_ShouldThrowException()
    {
        // Arrange
        var request = new CreateCourierDto
        {
            Id = "COU001",
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/cnh.png"
        };

        var existingCourier = new Courier { CnhNumber = request.CnhNumber };

        // Setup validator to return success (validation passes)
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockCreateCourierValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync((Courier?)null);

        _mockCourierRepository.Setup(x => x.GetByCnpjAsync(request.Cnpj))
            .ReturnsAsync((Courier?)null);

        _mockCourierRepository.Setup(x => x.GetByCnhNumberAsync(request.CnhNumber))
            .ReturnsAsync(existingCourier);

        // Act & Assert
        await _service.Invoking(x => x.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*CNH number already exists*");

        _mockCourierRepository.Verify(x => x.AddAsync(It.IsAny<Courier>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCnhImageAsync_ValidRequest_ShouldUpdateCnhImage()
    {
        // Arrange
        var courierId = "COU001";
        var request = new UpdateCnhImageDto
        {
            CnhImageUrl = "https://example.com/new-cnh.png"
        };

        var courier = new Courier
        {
            Id = courierId,
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/old-cnh.png"
        };

        var updatedCourier = new Courier
        {
            Id = courierId,
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = request.CnhImageUrl
        };

        var courierDto = new CourierDto
        {
            Id = courierId,
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = request.CnhImageUrl
        };

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateCnhImageValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(courierId))
            .ReturnsAsync(courier);

        _mockCourierRepository.Setup(x => x.UpdateAsync(It.IsAny<Courier>()))
            .ReturnsAsync(updatedCourier);

        _mockMapper.Setup(x => x.Map<CourierDto>(It.IsAny<Courier>()))
            .Returns(courierDto);

        // Act
        var result = await _service.UpdateCnhImageAsync(courierId, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(courierId);
        result.CnhImageUrl.Should().Be(request.CnhImageUrl);

        _mockCourierRepository.Verify(x => x.UpdateAsync(It.IsAny<Courier>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCnhImageAsync_NonExistingCourier_ShouldThrowException()
    {
        // Arrange
        var courierId = "NONEXISTENT";
        var request = new UpdateCnhImageDto
        {
            CnhImageUrl = "https://example.com/new-cnh.png"
        };

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateCnhImageValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(courierId))
            .ReturnsAsync((Courier?)null);

        // Act & Assert
        await _service.Invoking(x => x.UpdateCnhImageAsync(courierId, request))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not found*");

        _mockCourierRepository.Verify(x => x.UpdateAsync(It.IsAny<Courier>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCnhImageAsync_InvalidImageFormat_ShouldThrowException()
    {
        // Arrange
        var courierId = "COU001";
        var request = new UpdateCnhImageDto
        {
            CnhImageUrl = "https://example.com/image.jpg" // Invalid format
        };

        var courier = new Courier
        {
            Id = courierId,
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/old-cnh.png"
        };

        // Setup validator to return validation failure for invalid format
        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("CnhImageUrl", "Image must be in PNG or BMP format"));
        _mockUpdateCnhImageValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await _service.Invoking(x => x.UpdateCnhImageAsync(courierId, request))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("*Image must be in PNG or BMP format*");

        _mockCourierRepository.Verify(x => x.UpdateAsync(It.IsAny<Courier>()), Times.Never);
    }

    [Theory]
    [InlineData("https://example.com/image.png")]
    [InlineData("https://example.com/image.PNG")]
    [InlineData("https://example.com/image.bmp")]
    [InlineData("https://example.com/image.BMP")]
    public async Task UpdateCnhImageAsync_ValidImageFormats_ShouldUpdateCnhImage(string imageUrl)
    {
        // Arrange
        var courierId = "COU001";
        var request = new UpdateCnhImageDto
        {
            CnhImageUrl = imageUrl
        };

        var courier = new Courier
        {
            Id = courierId,
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/old-cnh.png"
        };

        var updatedCourier = new Courier
        {
            Id = courierId,
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = request.CnhImageUrl
        };

        var courierDto = new CourierDto
        {
            Id = courierId,
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = request.CnhImageUrl
        };

        // Setup validator to return success
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateCnhImageValidator.Setup(x => x.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        _mockCourierRepository.Setup(x => x.GetByIdAsync(courierId))
            .ReturnsAsync(courier);

        _mockCourierRepository.Setup(x => x.UpdateAsync(It.IsAny<Courier>()))
            .ReturnsAsync(updatedCourier);

        _mockMapper.Setup(x => x.Map<CourierDto>(It.IsAny<Courier>()))
            .Returns(courierDto);

        // Act
        var result = await _service.UpdateCnhImageAsync(courierId, request);

        // Assert
        result.Should().NotBeNull();
        result.CnhImageUrl.Should().Be(imageUrl);

        _mockCourierRepository.Verify(x => x.UpdateAsync(It.IsAny<Courier>()), Times.Once);
    }
}
