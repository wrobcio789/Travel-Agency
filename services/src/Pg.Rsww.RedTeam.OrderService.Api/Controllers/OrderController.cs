using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.OrderService.Api.Models;
using OrderResponse = Pg.Rsww.RedTeam.OrderService.Api.Models.OrderResponse;

namespace Pg.Rsww.RedTeam.OrderService.Api.Controllers;

using OrderService = Application.Services.OrderService;
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
	public async Task<OrderResponse> PostMakeOrder([FromBody] OfferRequest offerRequest)
	{
		if (offerRequest == null)
		{
			return null;
		}
		
		var response = await _orderService.ProcessOrder(offerRequest);

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
		var customerId = "JWT";//TODO
		
		var orders = await _orderService.GetOrders(customerId);
		var orderListings = _mapper.Map<List<OrderListing>>(orders);
		return orderListings;
	}
}