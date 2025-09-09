// RentalsController - Controller responsible for managing rental operations
// Endpoints: Create rental, finalize rental, query values

using Microsoft.AspNetCore.Mvc;
using Moto.Api.DTOs.Rentals;
using Moto.Application.Interfaces;
using Moto.Application.DTOs.Rentals;
using AutoMapper;

namespace Moto.Api.Controllers;

[ApiController]
[Route("api/rentals")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;

    public RentalsController(IRentalService rentalService, IMapper mapper)
    {
        _rentalService = rentalService;
        _mapper = mapper;
    }

    /// Create a new rental
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRentalRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = _mapper.Map<CreateRentalDto>(request);

            var result = await _rentalService.CreateAsync(appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = _mapper.Map<RentalResponse>(result);

            return Created($"/api/rentals/{responseDto.Id}", responseDto); //status code 201
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); //status code 400
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(ex.Message); //status code 400
        }
    }

    /// Get a rental by ID
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var rental = await _rentalService.GetByIdAsync(id);
        
        if (rental == null)
        {
            return NotFound();
        }
        
        // Map from Application DTO to API DTO
        var responseDto = _mapper.Map<RentalResponse>(rental);
        
        return Ok(responseDto);
    }

    /// Finalize a rental (return)
    [HttpPut("{id:guid}/return")]
    public async Task<IActionResult> ReturnAsync(Guid id, [FromBody] ReturnRentalRequest request)
    {
        try
        {
            // Map from API DTO to Application DTO
            var appRequest = _mapper.Map<ReturnRentalDto>(request);

            var result = await _rentalService.ReturnAsync(id, appRequest);
            
            // Map from Application DTO to API DTO
            var responseDto = _mapper.Map<RentalResponse>(result);

            return Ok(responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(ex.Message); //status code 400
        }
    }
}
