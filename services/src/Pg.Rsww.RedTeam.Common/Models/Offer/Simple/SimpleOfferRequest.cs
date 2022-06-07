namespace Pg.Rsww.RedTeam.Common.Models.Offer.Simple;

public class SimpleOfferRequest
{
	public string TourId { get; set; }
	public People People { get; set; }
	public SimpleAccommodation Accommodation { get; set; }
	public Transportation TransportationTo { get; set; }
	public Transportation TransportationFrom { get; set; }
	public string PromoCode { get; set; }
}