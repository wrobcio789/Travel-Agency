using AutoMapper;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OfferService.Api.Mapping;

public class TransportProfile : Profile
{
	public TransportProfile()
	{
		CreateMap<TransportResponse, TransportEntity>()
			.ForMember(dest => dest.Arrival, act => act.MapFrom(src => src.Arrival))
			.ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
			.ForMember(dest => dest.Departure, act => act.MapFrom(src => src.Departure))
			.ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type));
	}
}