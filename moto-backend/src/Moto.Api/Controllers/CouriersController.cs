// CouriersController - Controller responsible for managing courier operations
// Endpoints: CRUD of couriers, queries and filters

using Microsoft.AspNetCore.Mvc;
using Moto.Api.DTOs.Couriers;
using Moto.Application.Interfaces;
using Moto.Application.DTOs.Couriers;
using AutoMapper;

namespace Moto.Api.Controllers;

[ApiController]
[Route("api/couriers")] //route for the controller

public class CouriersController : ControllerBase
{
    private readonly ICourierService _courierService;
    private readonly IMapper _mapper;

    public CouriersController(ICourierService courierService, IMapper mapper)
    {
        _courierService = courierService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] RegisterCourierRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = _mapper.Map<CreateCourierDto>(request);

            var result = await _courierService.CreateAsync(appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = _mapper.Map<CourierResponse>(result);

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
        var responseDto = _mapper.Map<CourierResponse>(courierResponse);
        
        return Ok(responseDto);
    }

    // Get method to get all couriers
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var courierResponses = await _courierService.GetAllAsync();
        
        // Map from Application DTOs to API DTOs
        var responseDtos = _mapper.Map<IEnumerable<CourierResponse>>(courierResponses);
        
        return Ok(responseDtos);
    }

    // Update courier profile
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCourierRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = _mapper.Map<UpdateCourierDto>(request);

            var result = await _courierService.UpdateAsync(id, appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = _mapper.Map<CourierResponse>(result);

            return Ok(responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update CNH image - specific endpoint as per README
    [HttpPut("{id:guid}/cnh")]
    public async Task<IActionResult> UpdateCnhImageAsync(Guid id, [FromBody] UpdateCnhImageRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = _mapper.Map<UpdateCnhImageDto>(request);

            var result = await _courierService.UpdateCnhImageAsync(id, appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = _mapper.Map<CourierResponse>(result);

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