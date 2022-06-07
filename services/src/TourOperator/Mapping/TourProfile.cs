using System.Globalization;
using AutoMapper;
using TourOperator.Models.Api;
using TourOperator.Models.Entities;

namespace TourOperator.Mapping;

public class TourProfile : Profile
{
	public TourProfile()
	{
		CreateMap<Tour, TourEntity>()
			.ForMember(dest => dest.Arrival, act => act.MapFrom(src => src.Region))
			.ForMember(dest => dest.Price, act => act.MapFrom(src => src.Price))
			.ForMember(dest => dest.StartDate, act => act.MapFrom(src => ParseDate(src.Start)))
			.ForMember(dest => dest.EndDate, act => act.MapFrom(src => ParseDate(src.End)))
			.ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
			.ForMember(dest => dest.City, act => act.MapFrom(src => src.City))
			.ForMember(dest => dest.Country, act => act.MapFrom(src => src.Country));
	}

	private DateTime ParseDate(string date)
	{
		var success =DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var datetime);
		return success
			? datetime
			: DateTime.MinValue;
	}
}