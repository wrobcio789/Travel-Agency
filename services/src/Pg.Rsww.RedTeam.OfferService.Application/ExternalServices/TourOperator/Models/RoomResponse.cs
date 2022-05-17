using Pg.Rsww.RedTeam.Common.Models.Offer;

namespace Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models;

public class RoomResponse
{
	public RoomSize Size { get; set; }
	
	public int RoomCount { get; set; }
	
	public bool NewbornsFriendly { get; set; }
	
	public List<string> Amenities { get; set; }
}