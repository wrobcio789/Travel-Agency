using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.EventHandler.Services;
using Pg.Rsww.RedTeam.OrderService.Application.Models;
using Pg.Rsww.RedTeam.OrderService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OrderService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OrderService.Application.Services;

public class OrderService
{
	private readonly OrderRepository _orderRepository;
	private readonly RpcClientService _rpcClientService;
	private readonly QueueSendService _queueSendService;
	private readonly ILogger<OrderService> _logger;

	public OrderService(
		OrderRepository orderRepository,
		RpcClientService rpcClientService,
		QueueSendService queueSendService,
		ILogger<OrderService> logger
	)
	{
		_orderRepository = orderRepository;
		_rpcClientService = rpcClientService;
		_queueSendService = queueSendService;
		_logger = logger;
	}

	public async Task<OrderResponse> ProcessOrder(OfferRequest offer, string customerId)
	{
		var reservation = MakeReservation(offer);
		if (reservation is not { IsReserved: true })
		{
			_logger.Log(LogLevel.Information, "Reservation is not available");
			return null;
		}
		
		var orderId = await _orderRepository.CreateAsync(reservation.OfferId, customerId);


		var paymentId = InitPayment(reservation.Price, orderId);
		if (paymentId == null)
		{
			return null;
		}

		await _orderRepository.AddPaymentToOrder(orderId, paymentId);


		return new OrderResponse
		{
			OrderId = orderId,
			PaymentId = paymentId,
			Price = reservation.Price
		};
	}

	private string InitPayment(decimal Price, string orderId)
	{
		_logger.Log(LogLevel.Information, $"Init payment for order {orderId}");
		const string paymentQueueName = "create-payment";
		var paymentObj = new InitPaymentRequest
		{
			OrderId = orderId,
			Price = new Money
			{
				Value = Price,
				Currency = "PLN"
			}
		};
		var message = JsonConvert.SerializeObject(paymentObj,
			Formatting.Indented,
			new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
		var paymentId = _rpcClientService.Call(message, paymentQueueName);
		_logger.Log(LogLevel.Information, $"Received payment {paymentId} for order {orderId}");
		return paymentId.Replace("\"", string.Empty);
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
		var orders = await _orderRepository.GetAsync(customerId);
		return orders;
	}
}