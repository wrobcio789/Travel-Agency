namespace Pg.Rsww.RedTeam.Common.Models.Offer;

public class OfferReservationResponse
{
	public string OfferId { get; set; }
	public decimal Price { get; set; }
	public bool IsReserved { get; set; }
}