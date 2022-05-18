namespace Pg.Rsww.RedTeam.Common.Models.Offer.Request;

public class SimpleAccommodation
{
	public string HotelId { get; set; }
	public int SmallRooms { get; set; }
	public int MediumRooms { get; set; }
	public int LargeRooms { get; set; }
	public int NumberOfMeals { get; set; }
}