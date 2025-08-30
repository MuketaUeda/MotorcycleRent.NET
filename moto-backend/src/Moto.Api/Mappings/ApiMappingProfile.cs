// ApiMappingProfile - Perfil de mapeamento do AutoMapper para a camada da API
// Configurações de mapeamento entre DTOs da API e DTOs da Application

using AutoMapper;
using Moto.Api.DTOs.Motorcycles;
using Moto.Api.DTOs.Couriers;
using Moto.Api.DTOs.Rentals;
using Moto.Application.DTOs.Motorcycles;
using Moto.Application.DTOs.Couriers;
using Moto.Application.DTOs.Rentals;

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
        CreateMap<UpdateCourierRequest, UpdateCourierDto>();
        CreateMap<UpdateCnhImageRequest, UpdateCnhImageDto>();
        CreateMap<CourierDto, CourierResponse>()
            .ForMember(dest => dest.CnhType, opt => opt.MapFrom(src => src.CnhType.ToString()));

        // Rental mappings - API to Application
        CreateMap<CreateRentalRequest, CreateRentalDto>();
        CreateMap<ReturnRentalRequest, ReturnRentalDto>();
        CreateMap<RentalDto, RentalResponse>();
    }
}
