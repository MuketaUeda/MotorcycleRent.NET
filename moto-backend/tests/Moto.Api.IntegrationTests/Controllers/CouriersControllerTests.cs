// CouriersControllerTests - Integration tests for CouriersController
// Tests validation of courier data and CNH image updates
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
    public async Task Post_CreateCourier_InvalidCnpj_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            Id = "COU002",
            Name = "Maria Santos",
            Cnpj = "123456789", // Invalid CNPJ - too short
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "123456789",
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "https://example.com/cnh.png"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateCourier_InvalidAge_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            Id = "COU003",
            Name = "Pedro Junior",
            Cnpj = "12345678000199",
            BirthDate = new DateTime(2010, 1, 1), // Invalid - under 18
            CnhNumber = "123456789",
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "https://example.com/cnh.png"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateCourier_InvalidCnhNumber_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            Id = "COU004",
            Name = "Ana Costa",
            Cnpj = "12345678000199",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "12345", // Invalid CNH - too short
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "https://example.com/cnh.png"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateCourier_InvalidCnhImageUrl_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            Id = "COU005",
            Name = "Carlos Lima",
            Cnpj = "12345678000199",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "123456789",
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "invalid-url" // Invalid URL
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_CreateCourier_EmptyName_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            Id = "COU006",
            Name = "", // Empty name
            Cnpj = "12345678000199",
            BirthDate = new DateTime(1990, 1, 1),
            CnhNumber = "123456789",
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "https://example.com/cnh.png"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_UpdateCnhImage_ValidRequest_ShouldReturnOk()
    {
        // First create a courier
        var createRequest = new
        {
            Id = "COU007",
            Name = "Roberto Silva",
            Cnpj = "98765432000188",
            BirthDate = new DateTime(1985, 5, 15),
            CnhNumber = "987654321",
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "https://example.com/cnh.png"
        };

        await _client.PostAsJsonAsync("/api/couriers", createRequest);

        // Then update CNH image
        var updateRequest = new
        {
            CnhImageUrl = "https://example.com/new-cnh.png"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers/COU007/cnh", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_UpdateCnhImage_InvalidImageUrl_ShouldReturnBadRequest()
    {
        // First create a courier
        var createRequest = new
        {
            Id = "COU008",
            Name = "Fernanda Costa",
            Cnpj = "11111111000111",
            BirthDate = new DateTime(1988, 3, 20),
            CnhNumber = "111111111",
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "https://example.com/cnh.png"
        };

        await _client.PostAsJsonAsync("/api/couriers", createRequest);

        // Then try to update with invalid image URL
        var updateRequest = new
        {
            CnhImageUrl = "not-a-valid-url"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers/COU008/cnh", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_UpdateCnhImage_InvalidImageFormat_ShouldReturnBadRequest()
    {
        // First create a courier
        var createRequest = new
        {
            Id = "COU009",
            Name = "Lucas Oliveira",
            Cnpj = "22222222000222",
            BirthDate = new DateTime(1992, 7, 10),
            CnhNumber = "222222222",
            CnhType = 0, // A = 0, B = 1, AB = 2
            CnhImageUrl = "https://example.com/cnh.png"
        };

        await _client.PostAsJsonAsync("/api/couriers", createRequest);

        // Then try to update with invalid image format
        var updateRequest = new
        {
            CnhImageUrl = "https://example.com/cnh.jpg" // Invalid format - only PNG/BMP allowed
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/couriers/COU009/cnh", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_UpdateCnhImage_NonExistingId_ShouldReturnBadRequest()
    {
        // Arrange
        var nonExistingId = "NONEXISTENT";
        var request = new
        {
            CnhImageUrl = "https://example.com/cnh.png"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/couriers/{nonExistingId}/cnh", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}