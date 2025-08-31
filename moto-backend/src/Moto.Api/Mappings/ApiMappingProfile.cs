// ApiMappingProfile - Perfil de mapeamento do AutoMapper para a camada da API
// Configurações de mapeamento entre DTOs da API e DTOs da Application

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
        CreateMap<RegisterCourierRequest, CreateCourierDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Identifier))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.CnhNumber, opt => opt.MapFrom(src => src.LicenseNumber))
            .ForMember(dest => dest.CnhType, opt => opt.MapFrom(src => src.LicenseType))
            .ForMember(dest => dest.CnhImageUrl, opt => opt.MapFrom(src => src.LicenseImage));
        CreateMap<UpdateCnhImageRequest, UpdateCnhImageDto>()
            .ForMember(dest => dest.CnhImageUrl, opt => opt.MapFrom(src => src.LicenseImage));
        CreateMap<CourierDto, CourierResponse>()
            .ForMember(dest => dest.LicenseNumber, opt => opt.MapFrom(src => src.CnhNumber))
            .ForMember(dest => dest.LicenseType, opt => opt.MapFrom(src => src.CnhType.ToString()))
            .ForMember(dest => dest.LicenseImage, opt => opt.MapFrom(src => src.CnhImageUrl));

        // Rental mappings - API to Application
        CreateMap<CreateRentalRequest, CreateRentalDto>()
            .ForMember(dest => dest.MotorcycleId, opt => opt.MapFrom(src => src.MotorcycleId))
            .ForMember(dest => dest.CourierId, opt => opt.MapFrom(src => src.CourierId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.ExpectedEndDate, opt => opt.MapFrom(src => src.ExpectedEndDate))
            .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => (RentalPlan)src.Plan));
        CreateMap<ReturnRentalRequest, ReturnRentalDto>();
        CreateMap<RentalDto, RentalResponse>();
    }
}
