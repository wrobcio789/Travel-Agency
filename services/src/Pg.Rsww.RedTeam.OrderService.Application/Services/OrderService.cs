using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.EventHandler.Services;
using Pg.Rsww.RedTeam.OrderService.Application.Models;
using Pg.Rsww.RedTeam.OrderService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OrderService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OrderService.Application.Services;

public class OrderService
{
	private readonly OrderRepository _orderDbService;
	private readonly RpcClientService _rpcClientService;
	private readonly QueueSendService _queueSendService;
	private readonly ILogger<OrderService> _logger;

	public OrderService(
		OrderRepository orderDBService,
		RpcClientService rpcClientService,
		QueueSendService queueSendService,
		ILogger<OrderService> logger
	)
	{
		_orderDbService = orderDBService;
		_rpcClientService = rpcClientService;
		_queueSendService = queueSendService;
		_logger = logger;
	}

	public async Task<OrderResponse> ProcessOrder(OfferRequest offer)
	{
		var reservation = MakeReservation(offer);
		if (reservation is not { IsReserved: true })
		{
			return null;
		}

		var customerId = "JWT"; //TODO
		var orderId = await _orderDbService.CreateAsync(reservation.OfferId, customerId);


		var paymentId = InitPayment(reservation.Price, orderId);
		if (paymentId == null)
		{
			return null;
		}

		await _orderDbService.AddPaymentToOrder(orderId, paymentId);


		return new OrderResponse
		{
			OrderId = orderId,
			PaymentId = paymentId,
			Price = reservation.Price
		};
	}

	private string InitPayment(decimal Price, string orderId)
	{
		const string paymentQueueName = "init-payment";
		var paymentObj = new InitPaymentRequest
		{
			OrderId = orderId,
			Price = new Money
			{
				Value = Price,
				Currency = "PLN"
			}
		};
		var message = JsonConvert.SerializeObject(paymentObj);
		// var paymentId = _rpcClientService.Call(message, paymentQueueName);
		var paymentId = "payId";
		return paymentId;
	}

	private OfferReservationResponse MakeReservation(OfferRequest offer)
	{
		const string offerQueueName = "reserve-order";
		var message = JsonConvert.SerializeObject(offer);
		var response = _rpcClientService.Call(message, offerQueueName);
		var responseObj = JsonConvert.DeserializeObject<OfferReservationResponse>(response);
		return responseObj;
	}

	public async Task<List<OrderEntity>> GetOrders(string customerId)
	{
		var orders = await _orderDbService.GetAsync(customerId);
		return orders;
	}
}