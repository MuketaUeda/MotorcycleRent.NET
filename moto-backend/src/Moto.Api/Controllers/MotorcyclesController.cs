// MotorcyclesController - Controller responsible for managing motorcycle operations
// Endpoints: CRUD of motorcycles, queries and filters

using Microsoft.AspNetCore.Mvc;
using Moto.Api.DTOs.Motorcycles;
using Moto.Application.Interfaces;
using Moto.Application.DTOs.Motorcycles;
using AutoMapper;

namespace Moto.Api.Controllers;

[ApiController]
[Route("api/motorcycles")] //route for the controller

public class MotorcyclesController : ControllerBase{

    private readonly IMotorcycleService _motorcycleService;
    private readonly IMapper _mapper;

    public MotorcyclesController(IMotorcycleService motorcycleService, IMapper mapper){
        _motorcycleService = motorcycleService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMotorcycleRequest request){
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = _mapper.Map<CreateMotorcycleDto>(request);

            var result = await _motorcycleService.CreateAsync(appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = _mapper.Map<MotorcycleResponse>(result);

            return Created($"/api/motorcycles/{responseDto.Id}", responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get method to get a motorcycle by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id){
        var motorcycleResponse = await _motorcycleService.GetByIdAsync(id);
        
        if (motorcycleResponse == null)
        {
            return NotFound();
        }
        
        // Map from Application DTO to API DTO
        var responseDto = _mapper.Map<MotorcycleResponse>(motorcycleResponse);
        
        return Ok(responseDto);
    }

    // Get method to get all motorcycles with optional plate filter
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] string? plate = null){
        var motorcycleResponses = await _motorcycleService.GetAllAsync(plate);
        
        // Map from Application DTOs to API DTOs
        var responseDtos = _mapper.Map<IEnumerable<MotorcycleResponse>>(motorcycleResponses);
        
        return Ok(responseDtos);
    }

    // Update a motorcycle
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateMotorcycleRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = _mapper.Map<UpdateMotorcycleDto>(request);

            var result = await _motorcycleService.UpdateAsync(id, appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = _mapper.Map<MotorcycleResponse>(result);

            return Ok(responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete a motorcycle
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            var result = await _motorcycleService.DeleteAsync(id);
            
            if (!result)
            {
                return NotFound();
            }
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}