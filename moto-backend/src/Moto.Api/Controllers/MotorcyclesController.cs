// MotorcyclesController - Controller responsible for managing motorcycle operations
// Endpoints: CRUD of motorcycles, queries and filters

using Microsoft.AspNetCore.Mvc;
using Moto.Api.DTOs.Motorcycles;
using Moto.Application.Services;
using Moto.Application.DTOs.Motorcycles;

namespace Moto.Api.Controllers;

[ApiController]
[Route("api/motorcycles")] //route for the controller

public class MotorcyclesController : ControllerBase{

    private readonly MotorcycleService _motorcycleService;

    public MotorcyclesController(MotorcycleService motorcycleService){
        _motorcycleService = motorcycleService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMotorcycleRequest request){
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = new CreateMotorcycleDto
            {
                Model = request.Model,
                Plate = request.Plate,
                Year = request.Year
            };

            var result = await _motorcycleService.CreateAsync(appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = new MotorcycleResponse
            {
                Id = result.Id,
                Model = result.Model,
                Plate = result.Plate,
                Year = result.Year
            };

            return Created($"/api/motorcycles/{responseDto.Id}", responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get method to get a motorcycle by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id){
        var motorcycleResponse = await _motorcycleService.GetByIdAsync(id);
        
        if (motorcycleResponse == null)
        {
            return NotFound();
        }
        
        // Map from Application DTO to API DTO
        var responseDto = new MotorcycleResponse
        {
            Id = motorcycleResponse.Id,
            Model = motorcycleResponse.Model,
            Plate = motorcycleResponse.Plate,
            Year = motorcycleResponse.Year
        };
        
        return Ok(responseDto);
    }

    // Get method to get all motorcycles
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(){
        var motorcycleResponses = await _motorcycleService.GetAllAsync();
        
        // Map from Application DTOs to API DTOs
        var responseDtos = motorcycleResponses.Select(motorcycleResponse => new MotorcycleResponse
        {
            Id = motorcycleResponse.Id,
            Model = motorcycleResponse.Model,
            Plate = motorcycleResponse.Plate,
            Year = motorcycleResponse.Year
        });
        
        return Ok(responseDtos);
    }
    
}