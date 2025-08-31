// CouriersController - Controller responsible for managing courier operations
// Endpoints: POST for registering couriers and updating CNH image

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

    // Update CNH image - specific endpoint as per README
    [HttpPost("{id}/cnh")]
    public async Task<IActionResult> UpdateCnhImageAsync(string id, [FromBody] UpdateCnhImageRequest request)
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
}