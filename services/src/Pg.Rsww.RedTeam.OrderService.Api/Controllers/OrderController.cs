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
public class OrderController : Controller
{
	private readonly OrderService _orderService;
	private readonly ILogger<OrderController> _logger;
	private readonly IMapper _mapper;

	public OrderController(
		OrderService orderService,
		IMapper mapper,
		ILogger<OrderController> logger
		)
	{
		_mapper = mapper;
		_orderService = orderService;
		_logger = logger;
	}


	[HttpPost("Make")]
	public async Task<OrderResponse> PostMakeOrder([FromBody] SimpleOfferRequest simpleOfferRequest)
	{
		var offerRequest = _mapper.Map<OfferRequest>(simpleOfferRequest);
		var customerId = (string)HttpContext.Items["CustomerId"];
		if (customerId == null)
		{
			return null;
		}
		if (offerRequest == null)
		{
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