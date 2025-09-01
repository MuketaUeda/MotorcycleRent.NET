// MotorcycleTests - Unit tests for Motorcycle entity
// Tests entity creation, validations and business rules
using FluentAssertions;
using Moto.Domain.Entities;

namespace Moto.Domain.UnitTests.Entities;

public class MotorcycleTests
{
    [Fact]
    public void Constructor_ValidData_ShouldCreateMotorcycle()
    {
        // Arrange
        var id = "MOT001";
        var plate = "ABC1234";
        var year = 2024;
        var model = "Honda CG 160";

        // Act
        var motorcycle = new Motorcycle
        {
            Id = id,
            Plate = plate,
            Year = year,
            Model = model
        };

        // Assert
        motorcycle.Should().NotBeNull();
        motorcycle.Id.Should().Be(id);
        motorcycle.Plate.Should().Be(plate);
        motorcycle.Year.Should().Be(year);
        motorcycle.Model.Should().Be(model);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Id_EmptyOrWhitespace_ShouldBeInvalid(string invalidId)
    {
        // Arrange & Act
        var motorcycle = new Motorcycle
        {
            Id = invalidId,
            Plate = "ABC1234",
            Year = 2024,
            Model = "Honda CG 160"
        };

        // Assert
        motorcycle.Id.Should().Be(invalidId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Plate_EmptyOrWhitespace_ShouldBeInvalid(string invalidPlate)
    {
        // Arrange & Act
        var motorcycle = new Motorcycle
        {
            Id = "MOT001",
            Plate = invalidPlate,
            Year = 2024,
            Model = "Honda CG 160"
        };

        // Assert
        motorcycle.Plate.Should().Be(invalidPlate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1800)]
    [InlineData(2030)]
    public void Year_InvalidYear_ShouldBeInvalid(int invalidYear)
    {
        // Arrange & Act
        var motorcycle = new Motorcycle
        {
            Id = "MOT001",
            Plate = "ABC1234",
            Year = invalidYear,
            Model = "Honda CG 160"
        };

        // Assert
        motorcycle.Year.Should().Be(invalidYear);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Model_EmptyOrWhitespace_ShouldBeInvalid(string invalidModel)
    {
        // Arrange & Act
        var motorcycle = new Motorcycle
        {
            Id = "MOT001",
            Plate = "ABC1234",
            Year = 2024,
            Model = invalidModel
        };

        // Assert
        motorcycle.Model.Should().Be(invalidModel);
    }

    [Fact]
    public void Motorcycle_ValidData_ShouldHaveCorrectProperties()
    {
        // Arrange
        var id = "MOT001";
        var plate = "ABC1234";
        var year = 2024;
        var model = "Honda CG 160";

        // Act
        var motorcycle = new Motorcycle
        {
            Id = id,
            Plate = plate,
            Year = year,
            Model = model
        };

        // Assert
        motorcycle.Should().NotBeNull();
        motorcycle.Id.Should().Be(id);
        motorcycle.Plate.Should().Be(plate);
        motorcycle.Year.Should().Be(year);
        motorcycle.Model.Should().Be(model);
    }

    [Fact]
    public void Motorcycle_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var motorcycle = new Motorcycle();

        // Assert
        motorcycle.Id.Should().Be(string.Empty);
        motorcycle.Plate.Should().Be(string.Empty);
        motorcycle.Year.Should().Be(0);
        motorcycle.Model.Should().Be(string.Empty);
    }
}
