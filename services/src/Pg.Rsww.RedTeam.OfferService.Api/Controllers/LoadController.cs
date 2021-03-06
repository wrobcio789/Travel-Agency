using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pg.Rsww.RedTeam.OfferService.Api.Hubs;
using Pg.Rsww.RedTeam.OfferService.Api.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Services;

namespace Pg.Rsww.RedTeam.OfferService.Api.Controllers;

[Controller]
[Route("api/[controller]")]
public class LoadController : Controller
{
	private readonly LoaderService _loaderService;
	private readonly IHubContext<OfferHub> _hubContext;
	private readonly IMapper _mapper;

	public LoadController(LoaderService loaderService, IHubContext<OfferHub> hubContext, IMapper mapper)
	{
		_loaderService = loaderService;
		_hubContext = hubContext;
		_mapper = mapper;
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
		var content = _mapper.Map<List<TourSearchResponse>>(result);
		await _hubContext.Clients.All.SendAsync("Message", "TourChange",
			JsonConvert.SerializeObject(content,
				Formatting.Indented,
				new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				}));

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