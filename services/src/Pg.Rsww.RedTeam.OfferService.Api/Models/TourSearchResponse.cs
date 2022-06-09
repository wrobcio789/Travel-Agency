namespace Pg.Rsww.RedTeam.OfferService.Api.Models;

public class TourSearchResponse
{
	public string Id { get; set; }
	public string Title { get; set; }
	public string Arrival { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public bool Enabled { get; set; }
}