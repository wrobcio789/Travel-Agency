namespace Pg.Rsww.RedTeam.OrderService.Application.Models;

public class OrderResponse
{
	public string OrderId { get; set; }
	public string PaymentId { get; set; }
	public decimal Price { get; set; }
}