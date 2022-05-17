namespace Pg.Rsww.RedTeam.Common.Models.Offer;

public class Accommodation
{
	public string HotelId { get; set; }
	public List<Room> Rooms { get; set; }
	public int NumberOfMeals { get; set; }
}