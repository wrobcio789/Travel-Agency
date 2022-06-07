using System.Globalization;
using AutoMapper;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using TourResponse = Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models.TourResponse;

namespace Pg.Rsww.RedTeam.OfferService.Api.Mapping;

public class TourProfile : Profile
{
	public TourProfile()
	{
		CreateMap<TourResponse, TourEntity>()
			.ForMember(dest => dest.Arrival, act => act.MapFrom(src => src.Region))
			.ForMember(dest => dest.Price, act => act.MapFrom(src => src.Price))
			.ForMember(dest => dest.StartDate, act => act.MapFrom(src => ParseDate(src.Start)))
			.ForMember(dest => dest.EndDate, act => act.MapFrom(src => ParseDate(src.End)))
			.ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
			.ForMember(dest => dest.City, act => act.MapFrom(src => src.City))
			.ForMember(dest => dest.Country, act => act.MapFrom(src => src.Country))
			.ForMember(dest => dest.Enabled, act => act.MapFrom(src => src.Enabled));

		CreateMap<TourEntity, Models.TourResponse>();
	}

	private DateTime ParseDate(string date)
	{
		var success =DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var datetime);
		return success
			? datetime
			: DateTime.MinValue;
	}
}