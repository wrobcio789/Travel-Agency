using AutoMapper;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.Common.Models.Offer.Request;
using Pg.Rsww.RedTeam.OrderService.Api.Models;
using Pg.Rsww.RedTeam.OrderService.Application.Models.Entities;
using SimpleAccommodation = Pg.Rsww.RedTeam.Common.Models.Offer.Request.SimpleAccommodation;

namespace Pg.Rsww.RedTeam.OrderService.Api.Mapping;

public class OrderProfile : Profile
{
	public OrderProfile()
	{
		CreateMap<OrderEntity, OrderListing>()
			.ForMember(dest => dest.OrderId, act => act.MapFrom(src => src.Id))
			.ForMember(dest => dest.OfferId, act => act.MapFrom(src => src.OfferId))
			.ForMember(dest => dest.PaymentId, act => act.MapFrom(src => src.PaymentId))
			.ForMember(dest => dest.CreatedAt, act => act.MapFrom(src => src.CreatedAt))
			.ForMember(dest => dest.Status, act => act.MapFrom(src => src.Status.ToString()));

		CreateMap<SimpleOfferRequest, OfferRequest>()
			.ForMember(dest => dest.Accommodation, act => act.MapFrom(src => src.Accommodation));
	}
}