namespace Pg.Rsww.RedTeam.OfferService.Api.Models;

public class HotelListingResponse
{
	public string Id { get; set; }
	public string Name { get; set; }
	public List<string> Amenities { get; set; }
}