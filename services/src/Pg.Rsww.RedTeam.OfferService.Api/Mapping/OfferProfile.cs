using AutoMapper;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OfferService.Api.Mapping;

public class OfferProfile : Profile
{
	public OfferProfile()
	{
		CreateMap<OfferRequest, OfferEntity>()
			.ForMember(dest => dest.TourId, act => act.MapFrom(src => src.TourId))
			.ForMember(dest => dest.Participants, act => act.MapFrom(src => src.Participants))
			.ForPath(dest => dest.Reservation.StartTransport,
				act => act.MapFrom(src => src.StartTransportId))
			.ForPath(dest => dest.Reservation.EndTransport,
				act => act.MapFrom(src => src.EndTransportId))
			.ForPath(dest => dest.Reservation.Accommodation, act => act.MapFrom(src => src.Accommodation));
	}
}