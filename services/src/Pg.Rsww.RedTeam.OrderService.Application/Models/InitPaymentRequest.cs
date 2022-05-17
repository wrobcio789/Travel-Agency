namespace Pg.Rsww.RedTeam.OrderService.Application.Models
{
	public class InitPaymentRequest
	{
		public string OrderId { get; set; }
		public Money Price { get; set; }
	}

	public class Money
	{
		public decimal Value { get; set; }
		public string Currency { get; set; }
	}
}