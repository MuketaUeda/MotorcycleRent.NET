// CourierService - Serviço de aplicação para entregadores
// Implementa casos de uso: cadastrar, atualizar perfil, listar, buscar por CNPJ/CNH
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Application.DTOs.Couriers;
using Moto.Application.Interfaces;
using AutoMapper;

namespace Moto.Application.Services;

public class CourierService : ICourierService
{
    private readonly ICourierRepository _courierRepository;
    private readonly IMapper _mapper;

    // Dependency injection
    public CourierService(ICourierRepository courierRepository, IMapper mapper)
    {
        _courierRepository = courierRepository;
        _mapper = mapper;
    }
    
    // Create a courier
    public async Task<CourierDto> CreateAsync(CreateCourierDto request)
    {
        // Verificar se já existe um courier com o mesmo CNPJ
        var existingCourierByCnpj = await _courierRepository.GetByCnpjAsync(request.Cnpj);
        if (existingCourierByCnpj != null)
        {
            throw new InvalidOperationException("A courier with this CNPJ already exists.");
        }

        // Verificar se já existe um courier com o mesmo número de CNH
        var existingCourierByCnh = await _courierRepository.GetByCnhNumberAsync(request.CnhNumber);
        if (existingCourierByCnh != null)
        {
            throw new InvalidOperationException("A courier with this CNH number already exists.");
        }

        var courier = _mapper.Map<Courier>(request);
        courier.Id = Guid.NewGuid();

        await _courierRepository.AddAsync(courier);
        
        return _mapper.Map<CourierDto>(courier);
    }

    // Get a courier by id
    public async Task<CourierDto?> GetByIdAsync(Guid id)
    {
        var courier = await _courierRepository.GetByIdAsync(id);
        if (courier == null)
        {
            return null;
        }

        return _mapper.Map<CourierDto>(courier);
    }

    // Get all couriers
    public async Task<IEnumerable<CourierDto>> GetAllAsync()
    {
        var couriers = await _courierRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CourierDto>>(couriers);
    }

    // Update courier profile
    public async Task<CourierDto> UpdateAsync(Guid id, UpdateCourierDto request)
    {
        var courier = await _courierRepository.GetByIdAsync(id);
        if (courier == null)
        {
            throw new InvalidOperationException("Courier not found.");
        }

        // Atualizar apenas os campos permitidos
        if (!string.IsNullOrEmpty(request.Name))
        {
            courier.Name = request.Name;
        }

        if (!string.IsNullOrEmpty(request.CnhImageUrl))
        {
            courier.CnhImageUrl = request.CnhImageUrl;
        }

        await _courierRepository.UpdateAsync(courier);

        return _mapper.Map<CourierDto>(courier);
    }

    // Update CNH image
    public async Task<CourierDto> UpdateCnhImageAsync(Guid id, UpdateCnhImageDto request)
    {
        var courier = await _courierRepository.GetByIdAsync(id);
        if (courier == null)
        {
            throw new InvalidOperationException("Courier not found.");
        }

        // Validar formato da imagem (PNG ou BMP)
        if (!IsValidImageFormat(request.CnhImageUrl))
        {
            throw new InvalidOperationException("Image must be in PNG or BMP format.");
        }

        // Atualizar apenas a foto da CNH
        courier.CnhImageUrl = request.CnhImageUrl;

        await _courierRepository.UpdateAsync(courier);

        return _mapper.Map<CourierDto>(courier);
    }

    // Delete a courier
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _courierRepository.DeleteAsync(id);
    }

    // Helper method to validate image format
    private static bool IsValidImageFormat(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return false;

        var lowerUrl = imageUrl.ToLowerInvariant();
        return lowerUrl.EndsWith(".png") || lowerUrl.EndsWith(".bmp");
    }
}
