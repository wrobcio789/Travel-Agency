using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.OfferService.Api.Hubs;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Services;

namespace Pg.Rsww.RedTeam.OfferService.Api.Controllers;

[Controller]
[Route("api/[controller]")]
public class LoadController : Controller
{
	private readonly LoaderService _loaderService;
	private readonly IHubContext<OfferHub> _hubContext;

	public LoadController(LoaderService loaderService, IHubContext<OfferHub> hubContext)
	{
		_loaderService = loaderService;
		_hubContext = hubContext;
	}

	/// <summary>
	/// Endpoint for forcing full data load from Tour Operator
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<bool> LoadAll()
	{
		var result = await _loaderService.FullLoadAsync();
		return result;
	}

	[HttpPost("TourDelta")]
	public async Task<bool> LoadDeltaTours([FromBody] List<TourEntity> tours)
	{
		var result = await _loaderService.LoadDelta(tours);
		await _hubContext.Clients.All.SendAsync("Message", "TourChange", JsonConvert.SerializeObject(result.Select(x=>x.Id)));

		return result.Any();
	}

	[HttpPost("TransportDelta")]
	public async Task<bool> LoadDeltaTransports([FromBody] List<TransportEntity> transports)
	{
		var result = await _loaderService.LoadDelta(transports);
		await _hubContext.Clients.All.SendAsync("Message", "TransportChange", JsonConvert.SerializeObject(result.Select(x=>x.Id)));

		return result.Any();
	}

	[HttpPost("HotelDelta")]
	public async Task<bool> LoadDeltaHotels([FromBody] List<HotelEntity> hotels)
	{
		var result = await _loaderService.LoadDelta(hotels);
		await _hubContext.Clients.All.SendAsync("Message", "HotelChange", JsonConvert.SerializeObject(result.Select(x => x.Id)));

		return result.Any();
	}
}