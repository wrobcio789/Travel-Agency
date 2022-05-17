namespace Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models;

public class HotelResponse
{
	public string Name { get; set; }
	
	public string Country { get; set; }
	
	public string City { get; set; }
	
	public string Region { get; set; }
	
	public int Rating { get; set; }

	public List<string> Amenities { get; set; }

	public List<RoomResponse> Rooms { get; set; }
}
