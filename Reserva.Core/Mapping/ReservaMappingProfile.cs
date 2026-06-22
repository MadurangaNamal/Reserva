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

        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<TicketCategory, TicketCategoryDto>();

        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<BookingItem, BookingItemDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.TicketCategory.Name));

        CreateMap<Waitlist, WaitlistDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
