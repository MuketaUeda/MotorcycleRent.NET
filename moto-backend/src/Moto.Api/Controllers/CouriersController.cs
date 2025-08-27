// CouriersController - Controller responsible for managing courier operations
// Endpoints: CRUD of couriers, queries and filters

using Microsoft.AspNetCore.Mvc;
using Moto.Api.DTOs.Couriers;
using Moto.Application.Services;
using Moto.Application.DTOs.Couriers;

namespace Moto.Api.Controllers;

[ApiController]
[Route("api/couriers")] //route for the controller

public class CouriersController : ControllerBase
{
    private readonly CourierService _courierService;

    public CouriersController(CourierService courierService)
    {
        _courierService = courierService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] RegisterCourierRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = new CreateCourierDto
            {
                Name = request.Name,
                Cnpj = request.Cnpj,
                BirthDate = request.BirthDate,
                CnhNumber = request.CnhNumber,
                CnhType = request.CnhType,
                CnhImageUrl = request.CnhImageUrl
            };

            var result = await _courierService.CreateAsync(appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = new CourierResponse
            {
                Id = result.Id,
                Name = result.Name,
                Cnpj = result.Cnpj,
                BirthDate = result.BirthDate,
                CnhNumber = result.CnhNumber,
                CnhType = result.CnhType.ToString(),
                CnhImageUrl = result.CnhImageUrl,
            };

            return Created($"/api/couriers/{responseDto.Id}", responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get method to get a courier by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var courierResponse = await _courierService.GetByIdAsync(id);
        
        if (courierResponse == null)
        {
            return NotFound();
        }
        
        // Map from Application DTO to API DTO
        var responseDto = new CourierResponse
        {
            Id = courierResponse.Id,
            Name = courierResponse.Name,
            Cnpj = courierResponse.Cnpj,
            BirthDate = courierResponse.BirthDate,
            CnhNumber = courierResponse.CnhNumber,
            CnhType = courierResponse.CnhType.ToString(),
            CnhImageUrl = courierResponse.CnhImageUrl,
        };
        
        return Ok(responseDto);
    }

    // Get method to get all couriers
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var courierResponses = await _courierService.GetAllAsync();
        
        // Map from Application DTOs to API DTOs
        var responseDtos = courierResponses.Select(courierResponse => new CourierResponse
        {
            Id = courierResponse.Id,
            Name = courierResponse.Name,
            Cnpj = courierResponse.Cnpj,
            BirthDate = courierResponse.BirthDate,
            CnhNumber = courierResponse.CnhNumber,
            CnhType = courierResponse.CnhType.ToString(),
            CnhImageUrl = courierResponse.CnhImageUrl,
        });
        
        return Ok(responseDtos);
    }

    // Update courier profile
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCourierRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = new UpdateCourierDto
            {
                Name = request.Name,
                CnhImageUrl = request.CnhImageUrl
            };

            var result = await _courierService.UpdateAsync(id, appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = new CourierResponse
            {
                Id = result.Id,
                Name = result.Name,
                Cnpj = result.Cnpj,
                BirthDate = result.BirthDate,
                CnhNumber = result.CnhNumber,
                CnhType = result.CnhType.ToString(),
                CnhImageUrl = result.CnhImageUrl,
            };

            return Ok(responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete a courier
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _courierService.DeleteAsync(id);
        
        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}