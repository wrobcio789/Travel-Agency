namespace Pg.Rsww.RedTeam.Common.Models.Offer.Request;

public class SimpleOfferRequest
{
	public string TourId { get; set; }
	public People Participants { get; set; }
	public SimpleAccommodation Accommodation { get; set; }
	public string StartTransportId { get; set; }
	public string EndTransportId { get; set; }
	public string PromoCode { get; set; }
}