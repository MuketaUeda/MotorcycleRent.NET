// ICourierService - Interface of the courier service
// Define contracts for courier business operations
using Moto.Application.DTOs.Couriers;

namespace Moto.Application.Interfaces;

public interface ICourierService
{
    /// Create a new courier
    Task<CourierDto> CreateAsync(CreateCourierDto request);

    /// Update the CNH image of a courier
    Task<CourierDto> UpdateCnhImageAsync(string id, UpdateCnhImageDto request);
}
