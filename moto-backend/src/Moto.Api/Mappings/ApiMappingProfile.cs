// ApiMappingProfile - Mapping profile for AutoMapper between API DTOs and Application DTOs
// Mapping configurations between API DTOs and Application DTOs

using AutoMapper;
using Moto.Api.DTOs.Motorcycles;
using Moto.Api.DTOs.Couriers;
using Moto.Api.DTOs.Rentals;
using Moto.Application.DTOs.Motorcycles;
using Moto.Application.DTOs.Couriers;
using Moto.Application.DTOs.Rentals;
using Moto.Domain.Enums;

namespace Moto.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // Motorcycle mappings - API to Application
        CreateMap<CreateMotorcycleRequest, CreateMotorcycleDto>();
        CreateMap<UpdateMotorcycleRequest, UpdateMotorcycleDto>();
        CreateMap<MotorcycleDto, MotorcycleResponse>();

        // Courier mappings - API to Application
        CreateMap<RegisterCourierRequest, CreateCourierDto>();
        CreateMap<UpdateCnhImageRequest, UpdateCnhImageDto>();
        CreateMap<CourierDto, CourierResponse>();

        // Rental mappings - API to Application
        CreateMap<CreateRentalRequest, CreateRentalDto>();
        CreateMap<ReturnRentalRequest, ReturnRentalDto>();
        CreateMap<RentalDto, RentalResponse>();
    }
}
