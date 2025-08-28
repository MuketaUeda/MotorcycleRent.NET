// CourierService - Serviço de aplicação para entregadores
// Implementa casos de uso: cadastrar, atualizar perfil, listar, buscar por CNPJ/CNH
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Application.DTOs.Couriers;
using Moto.Application.Interfaces;

namespace Moto.Application.Services;

public class CourierService : ICourierService
{
    private readonly ICourierRepository _courierRepository;

    // Dependency injection
    public CourierService(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
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

        var courier = new Courier
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Cnpj = request.Cnpj,
            BirthDate = request.BirthDate,
            CnhNumber = request.CnhNumber,
            CnhType = request.CnhType,
            CnhImageUrl = request.CnhImageUrl
        };

        await _courierRepository.AddAsync(courier);
        
        return new CourierDto
        {
            Id = courier.Id,
            Name = courier.Name,
            Cnpj = courier.Cnpj,
            BirthDate = courier.BirthDate,
            CnhNumber = courier.CnhNumber,
            CnhType = courier.CnhType,
            CnhImageUrl = courier.CnhImageUrl
        };
    }

    // Get a courier by id
    public async Task<CourierDto?> GetByIdAsync(Guid id)
    {
        var courier = await _courierRepository.GetByIdAsync(id);
        if (courier == null)
        {
            return null;
        }

        return new CourierDto
        {
            Id = courier.Id,
            Name = courier.Name,
            Cnpj = courier.Cnpj,
            BirthDate = courier.BirthDate,
            CnhNumber = courier.CnhNumber,
            CnhType = courier.CnhType,
            CnhImageUrl = courier.CnhImageUrl
        };
    }

    // Get all couriers
    public async Task<IEnumerable<CourierDto>> GetAllAsync()
    {
        var couriers = await _courierRepository.GetAllAsync();
        
        return couriers.Select(courier => new CourierDto
        {
            Id = courier.Id,
            Name = courier.Name,
            Cnpj = courier.Cnpj,
            BirthDate = courier.BirthDate,
            CnhNumber = courier.CnhNumber,
            CnhType = courier.CnhType,
            CnhImageUrl = courier.CnhImageUrl
        });
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

        return new CourierDto
        {
            Id = courier.Id,
            Name = courier.Name,
            Cnpj = courier.Cnpj,
            BirthDate = courier.BirthDate,
            CnhNumber = courier.CnhNumber,
            CnhType = courier.CnhType,
            CnhImageUrl = courier.CnhImageUrl
        };
    }

    // Delete a courier
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _courierRepository.DeleteAsync(id);
    }
}
