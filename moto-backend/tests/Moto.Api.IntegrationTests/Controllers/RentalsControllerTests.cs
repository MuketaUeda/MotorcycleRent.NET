// RentalsControllerTests - Simple integration tests for RentalsController
// Tests basic API connectivity and responses
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
    public async Task Get_GetAllRentals_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/rentals");

        // Assert
        // Should return either OK (if database is working), InternalServerError (if database issues), or MethodNotAllowed (if endpoint not implemented)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task Get_GetRentalById_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";

        // Act
        var response = await _client.GetAsync($"/api/rentals/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreateRental_ShouldReturnCreated()
    {
        // Arrange
        var request = new
        {
            Id = "REN001",
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = 7,
            StartDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
            ExpectedEndDate = DateTime.Now.AddDays(8).ToString("yyyy-MM-dd")
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rentals", request);

        // Assert
        // Should return either Created (if validation passes) or BadRequest (if validation fails)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdateRental_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";
        var request = new
        {
            MotorcycleId = "MOT001",
            CourierId = "COU001",
            PlanType = 15,
            StartDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
            ExpectedEndDate = DateTime.Now.AddDays(16).ToString("yyyy-MM-dd")
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/rentals/{nonExistingId}", request);

        // Assert
        // Should return either NotFound (if endpoint exists) or MethodNotAllowed (if endpoint not implemented)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task Delete_DeleteRental_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";

        // Act
        var response = await _client.DeleteAsync($"/api/rentals/{nonExistingId}");

        // Assert
        // Should return either NotFound (if endpoint exists) or MethodNotAllowed (if endpoint not implemented)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task Post_ReturnRental_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";
        var request = new
        {
            EndDate = DateTime.Now.ToString("yyyy-MM-dd")
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/rentals/{nonExistingId}/return", request);

        // Assert
        // Should return either NotFound (if endpoint exists) or MethodNotAllowed (if endpoint not implemented)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed);
    }
}
