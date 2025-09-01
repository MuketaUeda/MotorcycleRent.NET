// RentalTests - Unit tests for Rental entity
// Tests entity creation, validations and business rules
using FluentAssertions;
using Moto.Domain.Entities;
using Moto.Domain.Enums;

namespace Moto.Domain.UnitTests.Entities;

public class RentalTests
{
    [Fact]
    public void Constructor_ValidData_ShouldCreateRental()
    {
        // Arrange
        var id = Guid.NewGuid();
        var motorcycleId = "MOT001";
        var courierId = "COU001";
        var planType = RentalPlan.SevenDays;
        var startDate = DateTime.Now;
        var expectedEndDate = startDate.AddDays(7);
        var endDate = expectedEndDate;
        var totalCost = 210.00m;
        var fineAmount = 0.00m;
        var additionalDaysCost = 0.00m;
        var additionalDays = 0;

        // Act
        var rental = new Rental
        {
            Id = id,
            MotorcycleId = motorcycleId,
            CourierId = courierId,
            PlanType = planType,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            EndDate = endDate,
            TotalCost = totalCost,
            FineAmount = fineAmount,
            AdditionalDaysCost = additionalDaysCost,
            AdditionalDays = additionalDays
        };

        // Assert
        rental.Should().NotBeNull();
        rental.Id.Should().Be(id);
        rental.MotorcycleId.Should().Be(motorcycleId);
        rental.CourierId.Should().Be(courierId);
        rental.PlanType.Should().Be(planType);
        rental.StartDate.Should().Be(startDate);
        rental.ExpectedEndDate.Should().Be(expectedEndDate);
        rental.EndDate.Should().Be(endDate);
        rental.TotalCost.Should().Be(totalCost);
        rental.FineAmount.Should().Be(fineAmount);
        rental.AdditionalDaysCost.Should().Be(additionalDaysCost);
        rental.AdditionalDays.Should().Be(additionalDays);
    }

    [Fact]
    public void Rental_ActiveRental_ShouldHaveNullEndDate()
    {
        // Arrange
        var startDate = DateTime.Now;
        var expectedEndDate = startDate.AddDays(7);

        // Act
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            EndDate = null, // Active rental
            TotalCost = null,
            FineAmount = null,
            AdditionalDaysCost = null,
            AdditionalDays = null
        };

        // Assert
        rental.EndDate.Should().BeNull();
        rental.TotalCost.Should().BeNull();
        rental.FineAmount.Should().BeNull();
        rental.AdditionalDaysCost.Should().BeNull();
        rental.AdditionalDays.Should().BeNull();
    }

    [Fact]
    public void Rental_CompletedRental_ShouldHaveCalculatedCosts()
    {
        // Arrange
        var startDate = DateTime.Now;
        var expectedEndDate = startDate.AddDays(7);
        var endDate = expectedEndDate.AddDays(2); // 2 days late
        var totalCost = 310.00m; // 7 days + 2 additional days
        var fineAmount = 0.00m;
        var additionalDaysCost = 100.00m; // 2 days * R$50
        var additionalDays = 2;

        // Act
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            EndDate = endDate,
            TotalCost = totalCost,
            FineAmount = fineAmount,
            AdditionalDaysCost = additionalDaysCost,
            AdditionalDays = additionalDays
        };

        // Assert
        rental.EndDate.Should().Be(endDate);
        rental.TotalCost.Should().Be(totalCost);
        rental.FineAmount.Should().Be(fineAmount);
        rental.AdditionalDaysCost.Should().Be(additionalDaysCost);
        rental.AdditionalDays.Should().Be(additionalDays);
    }

    [Theory]
    [InlineData(RentalPlan.SevenDays)]
    [InlineData(RentalPlan.FifteenDays)]
    [InlineData(RentalPlan.ThirtyDays)]
    [InlineData(RentalPlan.FortyFiveDays)]
    [InlineData(RentalPlan.FiftyDays)]
    public void PlanType_ValidPlans_ShouldBeAccepted(RentalPlan planType)
    {
        // Arrange & Act
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = planType,
            StartDate = DateTime.Now,
            ExpectedEndDate = DateTime.Now.AddDays(7)
        };

        // Assert
        rental.PlanType.Should().Be(planType);
    }

    [Fact]
    public void Rental_WithFine_ShouldHaveFineAmount()
    {
        // Arrange
        var startDate = DateTime.Now;
        var expectedEndDate = startDate.AddDays(7);
        var endDate = expectedEndDate.AddDays(-2); // 2 days early
        var fineAmount = 42.00m; // 20% of 2 days cost

        // Act
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            EndDate = endDate,
            FineAmount = fineAmount
        };

        // Assert
        rental.FineAmount.Should().Be(fineAmount);
        rental.EndDate.Should().Be(endDate);
        rental.EndDate.Should().BeBefore(expectedEndDate);
    }

    [Fact]
    public void Rental_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var rental = new Rental();

        // Assert
        rental.Id.Should().Be(Guid.Empty);
        rental.MotorcycleId.Should().Be(string.Empty);
        rental.CourierId.Should().Be(string.Empty);
        rental.PlanType.Should().Be(default(RentalPlan));
        rental.StartDate.Should().Be(default(DateTime));
        rental.ExpectedEndDate.Should().Be(default(DateTime));
        rental.EndDate.Should().BeNull();
        rental.TotalCost.Should().BeNull();
        rental.FineAmount.Should().BeNull();
        rental.AdditionalDaysCost.Should().BeNull();
        rental.AdditionalDays.Should().BeNull();
    }

    [Fact]
    public void Rental_DateValidation_StartDateShouldBeBeforeExpectedEndDate()
    {
        // Arrange
        var startDate = DateTime.Now;
        var expectedEndDate = startDate.AddDays(7);

        // Act
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = RentalPlan.SevenDays,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate
        };

        // Assert
        rental.StartDate.Should().BeBefore(rental.ExpectedEndDate);
        rental.StartDate.Should().Be(startDate);
        rental.ExpectedEndDate.Should().Be(expectedEndDate);
    }
}

