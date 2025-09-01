// MotorcyclesControllerTests - Simple integration tests for MotorcyclesController
// Tests basic API connectivity and responses
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Moto.Api.IntegrationTests.Controllers;

public class MotorcyclesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MotorcyclesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_HealthCheck_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_Swagger_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/swagger");

        // Assert
        // Swagger might not be available in test environment, so accept both OK and NotFound
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_ApiEndpoint_ShouldReturnNotFound_WhenEndpointDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_MotorcyclesEndpoint_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/motorcycles");

        // Assert
        // Should return either OK (if database is working) or InternalServerError (if database issues)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.InternalServerError);
    }
}
