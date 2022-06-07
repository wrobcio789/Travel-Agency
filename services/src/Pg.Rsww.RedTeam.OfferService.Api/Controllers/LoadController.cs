using Microsoft.AspNetCore.Mvc;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Services;

namespace Pg.Rsww.RedTeam.OfferService.Api.Controllers;

[Controller]
[Route("api/[controller]")]
public class LoadController : Controller
{
	private readonly LoaderService _loaderService;

	public LoadController(LoaderService loaderService)
	{
		_loaderService = loaderService;
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

		return result;
	}

	[HttpPost("TransportDelta")]
	public async Task<bool> LoadDeltaTransports([FromBody] List<TransportEntity> transports)
	{
		var result = await _loaderService.LoadDelta(transports);

		return result;
	}

	[HttpPost("HotelDelta")]
	public async Task<bool> LoadDeltaHotels([FromBody] List<HotelEntity> hotels)
	{
		var result = await _loaderService.LoadDelta(hotels);

		return result;
	}
}