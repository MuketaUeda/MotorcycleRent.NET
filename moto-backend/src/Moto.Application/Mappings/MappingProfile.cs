// MappingProfile - Perfil de mapeamento do AutoMapper
// Configurações de mapeamento entre entidades e DTOs

using AutoMapper;
using Moto.Domain.Entities;
using Moto.Application.DTOs.Motorcycles;
using Moto.Application.DTOs.Couriers;
using Moto.Application.DTOs.Rentals;
using Moto.Application.DTOs.Events;

namespace Moto.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Motorcycle mappings
        CreateMap<Motorcycle, MotorcycleDto>().ReverseMap();
        CreateMap<Motorcycle, CreateMotorcycleDto>().ReverseMap();
        CreateMap<Motorcycle, UpdateMotorcycleDto>().ReverseMap();
        CreateMap<Motorcycle, MotorcycleCreatedEventDto>();

        // Courier mappings
        CreateMap<Courier, CourierDto>().ReverseMap();
        CreateMap<Courier, CreateCourierDto>().ReverseMap();
        CreateMap<Courier, UpdateCourierDto>().ReverseMap();
        CreateMap<Courier, UpdateCnhImageDto>().ReverseMap();

        // Rental mappings
        CreateMap<Rental, RentalDto>().ReverseMap();
        CreateMap<Rental, CreateRentalDto>().ReverseMap();
        CreateMap<Rental, ReturnRentalDto>().ReverseMap();
    }
}
