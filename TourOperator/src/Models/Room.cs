namespace TourOperator.Models;

public class Room
{
	public string Size { get; set; }
	
	public int RoomCount { get; set; }
	
	public bool NewbornsFriendly { get; set; }
	
	public List<string> Amenities { get; set; }
}