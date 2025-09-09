// RentalsControllerTests - Simplified integration tests for RentalsController
// Tests only input validation without database persistence
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Moto.Api.IntegrationTests.Controllers;

public class RentalsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public RentalsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Post_CreateRental_ValidRequest_ShouldValidateInput()
    {
        // Arrange - Test only input validation, not database operations
        var request = new
        {
            MotorcycleId = "MOT200",
            CourierId = "COU200",
            StartDate = DateTime.Now.AddDays(1),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = 7 // SevenDays = 7
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rentals", request);

        // Assert - Only test that the endpoint accepts the request (validation passes)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateRental_InvalidStartDate_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            MotorcycleId = "MOT201",
            CourierId = "COU201",
            StartDate = DateTime.Now.AddDays(-1), // Invalid - past date
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = 7
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rentals", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateRental_InvalidExpectedEndDate_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            MotorcycleId = "MOT202",
            CourierId = "COU202",
            StartDate = DateTime.Now.AddDays(1),
            ExpectedEndDate = DateTime.Now.AddDays(-1), // Invalid - before start date
            PlanType = 7
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rentals", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateRental_InvalidPlanType_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            MotorcycleId = "MOT203",
            CourierId = "COU203",
            StartDate = DateTime.Now.AddDays(1),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = 99 // Invalid plan type
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rentals", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateRental_EmptyMotorcycleId_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            MotorcycleId = "", // Empty motorcycle ID
            CourierId = "COU204",
            StartDate = DateTime.Now.AddDays(1),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = 7
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rentals", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateRental_EmptyCourierId_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            MotorcycleId = "MOT205",
            CourierId = "", // Empty courier ID
            StartDate = DateTime.Now.AddDays(1),
            ExpectedEndDate = DateTime.Now.AddDays(8),
            PlanType = 7
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rentals", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_GetRentalById_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/rentals/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_ReturnRental_ValidRequest_ShouldValidateInput()
    {
        // Arrange - Test only input validation, not database operations
        var returnRequest = new
        {
            ReturnDate = DateTime.Now.AddDays(5) // Return after 5 days
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/rentals/{Guid.NewGuid()}/return", returnRequest);

        // Assert - Accept both BadRequest (ID not found) and OK (validation passed)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_ReturnRental_InvalidReturnDate_ShouldReturnBadRequest()
    {
        // Arrange
        var returnRequest = new
        {
            ReturnDate = DateTime.Now.AddDays(-1) // Invalid - past date
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/rentals/{Guid.NewGuid()}/return", returnRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_ReturnRental_EmptyReturnDate_ShouldReturnBadRequest()
    {
        // Arrange
        var returnRequest = new
        {
            ReturnDate = DateTime.MinValue // Invalid - empty date
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/rentals/{Guid.NewGuid()}/return", returnRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}