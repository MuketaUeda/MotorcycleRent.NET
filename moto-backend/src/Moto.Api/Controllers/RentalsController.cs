// RentalsController - Controller responsável por gerenciar operações de locação
// Endpoints: Criar locação, finalizar locação, consultar valores

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

    /// <summary>
    /// Creates a new rental
    /// </summary>
    /// <param name="request">Rental creation data</param>
    /// <returns>Created rental</returns>
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

            return Created($"/api/rentals/{responseDto.Id}", responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets a rental by ID
    /// </summary>
    /// <param name="id">Rental ID</param>
    /// <returns>Found rental or 404 if not found</returns>
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

    /// <summary>
    /// Gets all rentals
    /// </summary>
    /// <returns>List of all rentals</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var rentals = await _rentalService.GetAllAsync();
        
        // Map from Application DTOs to API DTOs
        var responseDtos = _mapper.Map<IEnumerable<RentalResponse>>(rentals);
        
        return Ok(responseDtos);
    }

    /// <summary>
    /// Finalizes a rental (return)
    /// </summary>
    /// <param name="id">Rental ID</param>
    /// <param name="request">Return data</param>
    /// <returns>Finalized rental with calculated costs</returns>
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
            return BadRequest(ex.Message);
        }
    }
}
