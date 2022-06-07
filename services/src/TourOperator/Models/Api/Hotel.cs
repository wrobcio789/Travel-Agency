namespace TourOperator.Models.Api;

public class Hotel
{
	public string Name { get; set; }
	
	public string Country { get; set; }
	
	public string City { get; set; }
	
	public string Region { get; set; }
	
	public int Rating { get; set; }
	
	public List<string> Amenities { get; set; }
	
	public List<Room> Rooms { get; set; }
}
