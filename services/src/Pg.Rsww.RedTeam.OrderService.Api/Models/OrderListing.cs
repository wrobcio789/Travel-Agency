namespace Pg.Rsww.RedTeam.OrderService.Api.Models;

public class OrderListing
{
	public string OrderId { get; set; }

	public string OfferId { get; set; }

	public string PaymentId { get; set; }

	public DateTime CreatedAt { get; set; }
	
	public string Status { get; set; }
}