namespace TourOperator.Models.Api;

public class Tour
{
	public string Title { get; set; }
	
	public string Country { get; set; }
	
	public string City { get; set; }
	
	public string Region { get; set; }
	
	public int Price { get; set; }
	
	public int Rating { get; set; }
	
	public string Start { get; set; }
	
	public string End { get; set; }
	
	public int Days { get; set; }
	
	public bool Enabled { get; set; } = true;
}