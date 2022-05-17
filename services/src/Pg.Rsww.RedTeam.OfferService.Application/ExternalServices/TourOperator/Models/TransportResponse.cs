namespace Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models;

public class TransportResponse
{
	public string Name { get; set; }
	
	public string Departure { get; set; }
	
	public string Arrival { get; set; }
	
	public int Capacity { get; set; }
	
	public string Type { get; set; }
}