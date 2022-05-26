using AutoMapper;
using Pg.Rsww.RedTeam.OfferService.Api.Models;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OfferService.Api.Mapping
{
	public class HotelProfile : Profile
	{
		public HotelProfile()
		{
			CreateMap<HotelResponse, HotelEntity>()
				.ForMember(dest => dest.City, act => act.MapFrom(src => src.City))
				.ForMember(dest => dest.Country, act => act.MapFrom(src => src.Country))
				.ForMember(dest => dest.Region, act => act.MapFrom(src => src.Region))
				.ForMember(dest => dest.Rooms, act => act.MapFrom(src => src.Rooms))
				.ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name));
			
			CreateMap<RoomResponse, RoomEntity>()
				.ForMember(dest => dest.Amenities, act => act.MapFrom(src => src.Amenities))
				.ForMember(dest => dest.NewbornsFriendly, act => act.MapFrom(src => src.NewbornsFriendly))
				.ForMember(dest => dest.RoomCount, act => act.MapFrom(src => src.RoomCount))
				.ForMember(dest => dest.Size, act => act.MapFrom(src => src.Size));


			CreateMap<HotelEntity, HotelListingResponse>()
				.ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
				.ForMember(dest => dest.Amenities, act => act.MapFrom(src => src.Amenities));
		}
	}
}