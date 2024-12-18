using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<ColorDTO, Color>().ReverseMap();
            CreateMap<VehicleTypeDTO, VehicleType>().ReverseMap();
            CreateMap<VehicleTypeInputDTO, VehicleType>().ReverseMap();
            CreateMap<VehicleInputDTO, VehicleOutputDTO>().ReverseMap();

            // Mapeamento de Vehicle para VehicleOutputDTO
            CreateMap<Vehicle, VehicleOutputDTO>()
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.VehicleTypeId))
                .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.Name))
                .ForMember(dest => dest.VehicleTypePassengersCapacity, opt => opt.MapFrom(src => src.VehicleType.PassengersCapacity.ToString()))
                .ForMember(dest => dest.Chassis, opt => opt.MapFrom(src => src.Chassis))
                .ForMember(dest => dest.ChassisNumber, opt => opt.MapFrom(src => src.ChassisNumber.ToString()));

            // Mapeamento de VehicleInputDTO para Vehicle
            CreateMap<VehicleInputDTO, Vehicle>()
                .ForMember(dest => dest.ChassisNumber, opt => opt.MapFrom(src => long.Parse(src.ChassisNumber)))
                .ForMember(dest => dest.ChassisSeries, opt => opt.MapFrom(src => src.ChassisSeries));

            //CreateMap<Vehicle, VehicleOutputDTO>()
            //   .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
            //   .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
            //   .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.VehicleTypeId))
            //   .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.Name))
            //   .ForMember(dest => dest.Chassis, opt => opt.MapFrom(src => src.Chassis));
            //CreateMap<Vehicle, VehicleInputDTO>().ReverseMap();
        }
    }
}
