namespace Pg.Rsww.RedTeam.Common.Models.Offer;

public class OfferRequest
{
	public string TourId { get; set; }
	public People Participants { get; set; }
	public Accommodation Accommodation { get; set; }
	public string StartTransportId { get; set; }
	public Transportation TransportationTo { get; set; }
	public string EndTransportId { get; set; }
	public Transportation TransportationFrom { get; set; }
	public string PromoCode { get; set; }
}