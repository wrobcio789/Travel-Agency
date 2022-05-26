using Pg.Rsww.RedTeam.Common.Models.Offer;

namespace Pg.Rsww.RedTeam.OfferService.Api.Models;

public class TourRequest
{
	public string Arrival { get; set; }
	public string Departure { get; set; }
	public DateTime DepartureDate { get; set; }
	public People People { get; set; }
}