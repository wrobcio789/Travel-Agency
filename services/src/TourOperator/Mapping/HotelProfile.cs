using AutoMapper;
using TourOperator.Models.Api;
using TourOperator.Models.Entities;

namespace TourOperator.Mapping
{
	public class HotelProfile : Profile
	{
		public HotelProfile()
		{
			CreateMap<Hotel, HotelEntity>()
				.ForMember(dest => dest.City, act => act.MapFrom(src => src.City))
				.ForMember(dest => dest.Country, act => act.MapFrom(src => src.Country))
				.ForMember(dest => dest.Region, act => act.MapFrom(src => src.Region))
				.ForMember(dest => dest.Rooms, act => act.MapFrom(src => src.Rooms))
				.ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name));
			
			CreateMap<Room, RoomEntity>()
				.ForMember(dest => dest.Amenities, act => act.MapFrom(src => src.Amenities))
				.ForMember(dest => dest.NewbornsFriendly, act => act.MapFrom(src => src.NewbornsFriendly))
				.ForMember(dest => dest.RoomCount, act => act.MapFrom(src => src.RoomCount))
				.ForMember(dest => dest.Size, act => act.MapFrom(src => src.Size));
		}
	}
}