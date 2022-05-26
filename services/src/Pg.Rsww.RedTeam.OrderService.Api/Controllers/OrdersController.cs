using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.Common.Models.Offer.Request;
using Pg.Rsww.RedTeam.OrderService.Api.Middleware;
using Pg.Rsww.RedTeam.OrderService.Api.Models;
using OrderResponse = Pg.Rsww.RedTeam.OrderService.Api.Models.OrderResponse;

namespace Pg.Rsww.RedTeam.OrderService.Api.Controllers;

using OrderService = Application.Services.OrderService;
[Authorize]
[Controller]
[Route("api/[controller]")]
public class OrdersController : Controller
{
	private readonly OrderService _orderService;
	private readonly ILogger<OrdersController> _logger;
	private readonly IMapper _mapper;

	public OrdersController(
		OrderService orderService,
		IMapper mapper,
		ILogger<OrdersController> logger
		)
	{
		_mapper = mapper;
		_orderService = orderService;
		_logger = logger;
	}


	[HttpPost("Make")]
	public async Task<OrderResponse> PostMakeOrder([FromBody] SimpleOfferRequest simpleOfferRequest)
	{
		var customerId = (string)HttpContext.Items["CustomerId"];
		if (customerId == null)
		{
			_logger.Log(LogLevel.Information,"Customer not set");
			return null;
		}

		var offerRequest = _mapper.Map<OfferRequest>(simpleOfferRequest);
		if (offerRequest == null)
		{
			_logger.Log(LogLevel.Information, "Could not map simple offer request");
			return null;
		}
		
		var response = await _orderService.ProcessOrder(offerRequest,customerId);

		if (response == null)
		{
			return new OrderResponse();
		}

		return new OrderResponse
		{
			OrderId = response.OrderId,
			PaymentId = response.PaymentId,
			Price = response.Price
		};
	}

	[HttpGet("List")]
	public async Task<List<OrderListing>> PostListOrders()
	{
		var customerId = (string)HttpContext.Items["CustomerId"];
		if (customerId == null)
		{
			return null;
		}
		var orders = await _orderService.GetOrders(customerId);
		var orderListings = _mapper.Map<List<OrderListing>>(orders);
		return orderListings;
	}
}