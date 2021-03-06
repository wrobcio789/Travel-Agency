using AutoMapper;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.Common.Models.Offer.Simple;

namespace Pg.Rsww.RedTeam.Common.Mapping;

public class OfferProfile : Profile
{
	public OfferProfile()
	{
		CreateMap<SimpleOfferRequest, OfferRequest>()
			.ForMember(dest => dest.Accommodation, act => act.MapFrom(src => src.Accommodation));
		CreateMap<SimpleAccommodation, Accommodation>()
			.ForMember(dest => dest.Rooms, act => act.MapFrom(src => new List<Room>
			{
				new()
				{
					RoomSize = RoomSize.Small,
					Count = src.SmallRooms
				},
				new()
				{
					RoomSize = RoomSize.Medium,
					Count = src.MediumRooms
				},
				new()
				{
					RoomSize = RoomSize.Large,
					Count = src.LargeRooms
				}
			}));
	}
}