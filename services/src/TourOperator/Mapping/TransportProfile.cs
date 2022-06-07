using AutoMapper;
using TourOperator.Models.Api;
using TourOperator.Models.Entities;

namespace TourOperator.Mapping;

public class TransportProfile : Profile
{
	public TransportProfile()
	{
		CreateMap<Transport, TransportEntity>()
			.ForMember(dest => dest.Arrival, act => act.MapFrom(src => src.Arrival))
			.ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
			.ForMember(dest => dest.Departure, act => act.MapFrom(src => src.Departure))
			.ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type));
	}
}