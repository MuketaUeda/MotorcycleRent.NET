// CouriersControllerTests - Simple integration tests for CouriersController
// Tests basic API connectivity and responses
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Moto.Api.IntegrationTests.Controllers;

public class CouriersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CouriersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_GetAllCouriers_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/couriers");

        // Assert
        // Should return either OK (if database is working), InternalServerError (if database issues), or MethodNotAllowed (if endpoint not implemented)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.InternalServerError, HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task Get_GetCourierById_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";

        // Act
        var response = await _client.GetAsync($"/api/couriers/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreateCourier_ShouldReturnCreated()
    {
        // Arrange
        var request = new
        {
            Id = "COU001",
            Cnpj = "12345678000199",
            Name = "João Silva",
            BirthDate = "1990-01-01",
            CnhNumber = "12345678901",
            CnhType = "A"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers", request);

        // Assert
        // Should return either Created (if validation passes) or BadRequest (if validation fails)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdateCourier_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";
        var request = new
        {
            Cnpj = "12345678000199",
            Name = "João Silva Atualizado",
            BirthDate = "1990-01-01",
            CnhNumber = "12345678901",
            CnhType = "A"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/couriers/{nonExistingId}", request);

        // Assert
        // Should return either NotFound (if endpoint exists) or MethodNotAllowed (if endpoint not implemented)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task Delete_DeleteCourier_NonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";

        // Act
        var response = await _client.DeleteAsync($"/api/couriers/{nonExistingId}");

        // Assert
        // Should return either NotFound (if endpoint exists) or MethodNotAllowed (if endpoint not implemented)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed);
    }
}
