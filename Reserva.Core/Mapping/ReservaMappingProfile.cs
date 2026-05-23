using AutoMapper;
using Reserva.Contracts.DataContracts;
using Reserva.Data.Entities;

namespace Reserva.Core.Mapping;

public class ReservaMappingProfile : Profile
{
    public ReservaMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}
