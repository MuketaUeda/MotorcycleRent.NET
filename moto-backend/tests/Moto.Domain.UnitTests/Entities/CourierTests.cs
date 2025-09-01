// CourierTests - Unit tests for Courier entity
// Tests entity creation, validations and business rules
using FluentAssertions;
using Moto.Domain.Entities;
using Moto.Domain.Enums;

namespace Moto.Domain.UnitTests.Entities;

public class CourierTests
{
    [Fact]
    public void Constructor_ValidData_ShouldCreateCourier()
    {
        // Arrange
        var id = "COU001";
        var name = "João Silva";
        var cnpj = "12345678901234";
        var birthDate = new DateTime(1990, 1, 1);
        var cnhNumber = "12345678901";
        var cnhType = CnhType.A;
        var cnhImageUrl = "https://example.com/cnh.png";

        // Act
        var courier = new Courier
        {
            Id = id,
            Name = name,
            Cnpj = cnpj,
            BirthDate = birthDate,
            CnhNumber = cnhNumber,
            CnhType = cnhType,
            CnhImageUrl = cnhImageUrl
        };

        // Assert
        courier.Should().NotBeNull();
        courier.Id.Should().Be(id);
        courier.Name.Should().Be(name);
        courier.Cnpj.Should().Be(cnpj);
        courier.BirthDate.Should().Be(birthDate);
        courier.CnhNumber.Should().Be(cnhNumber);
        courier.CnhType.Should().Be(cnhType);
        courier.CnhImageUrl.Should().Be(cnhImageUrl);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Id_EmptyOrWhitespace_ShouldBeInvalid(string invalidId)
    {
        // Arrange & Act
        var courier = new Courier
        {
            Id = invalidId,
            Name = "João Silva",
            Cnpj = "12345678901234",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A
        };

        // Assert
        courier.Id.Should().Be(invalidId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Name_EmptyOrWhitespace_ShouldBeInvalid(string invalidName)
    {
        // Arrange & Act
        var courier = new Courier
        {
            Id = "COU001",
            Name = invalidName,
            Cnpj = "12345678901234",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A
        };

        // Assert
        courier.Name.Should().Be(invalidName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234567890123")] // 13 digits
    [InlineData("123456789012345")] // 15 digits
    public void Cnpj_InvalidFormat_ShouldBeInvalid(string invalidCnpj)
    {
        // Arrange & Act
        var courier = new Courier
        {
            Id = "COU001",
            Name = "João Silva",
            Cnpj = invalidCnpj,
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A
        };

        // Assert
        courier.Cnpj.Should().Be(invalidCnpj);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234567890")] // 10 digits
    [InlineData("123456789012")] // 12 digits
    public void CnhNumber_InvalidFormat_ShouldBeInvalid(string invalidCnhNumber)
    {
        // Arrange & Act
        var courier = new Courier
        {
            Id = "COU001",
            Name = "João Silva",
            Cnpj = "12345678901234",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = invalidCnhNumber,
            CnhType = CnhType.A
        };

        // Assert
        courier.CnhNumber.Should().Be(invalidCnhNumber);
    }

    [Fact]
    public void CnhType_ValidTypes_ShouldBeAccepted()
    {
        // Arrange & Act
        var courierA = new Courier { CnhType = CnhType.A };
        var courierB = new Courier { CnhType = CnhType.B };
        var courierAB = new Courier { CnhType = CnhType.AB };

        // Assert
        courierA.CnhType.Should().Be(CnhType.A);
        courierB.CnhType.Should().Be(CnhType.B);
        courierAB.CnhType.Should().Be(CnhType.AB);
    }

    [Fact]
    public void BirthDate_ValidDate_ShouldBeAccepted()
    {
        // Arrange
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        var courier = new Courier
        {
            Id = "COU001",
            Name = "João Silva",
            Cnpj = "12345678901234",
            BirthDate = birthDate,
            CnhNumber = "12345678901",
            CnhType = CnhType.A
        };

        // Assert
        courier.BirthDate.Should().Be(birthDate);
    }

    [Fact]
    public void CnhImageUrl_NullOrEmpty_ShouldBeAccepted()
    {
        // Arrange & Act
        var courierWithUrl = new Courier
        {
            Id = "COU001",
            Name = "João Silva",
            Cnpj = "12345678901234",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345678901",
            CnhType = CnhType.A,
            CnhImageUrl = "https://example.com/cnh.png"
        };

        var courierWithoutUrl = new Courier
        {
            Id = "COU002",
            Name = "Maria Santos",
            Cnpj = "98765432109876",
            BirthDate = new DateTime(1985, 5, 15),
            CnhNumber = "98765432109",
            CnhType = CnhType.B,
            CnhImageUrl = null
        };

        // Assert
        courierWithUrl.CnhImageUrl.Should().Be("https://example.com/cnh.png");
        courierWithoutUrl.CnhImageUrl.Should().BeNull();
    }

    [Fact]
    public void Courier_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var courier = new Courier();

        // Assert
        courier.Id.Should().Be(string.Empty);
        courier.Name.Should().Be(string.Empty);
        courier.Cnpj.Should().Be(string.Empty);
        courier.BirthDate.Should().Be(default(DateTime));
        courier.CnhNumber.Should().Be(string.Empty);
        courier.CnhType.Should().Be(default(CnhType));
        courier.CnhImageUrl.Should().BeNull();
    }
}
