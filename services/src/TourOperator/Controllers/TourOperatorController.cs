using Microsoft.AspNetCore.Mvc;
using TourOperator.Models.Api;
using TourOperator.Models.Entities;
using TourOperator.Services;
using TourOperator.Utils;

namespace TourOperator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TourOperatorController : ControllerBase
{
	private readonly LoaderService _loaderService;
	private readonly ILogger<TourOperatorController> _logger;

	public TourOperatorController(
		LoaderService loaderService,
		ILogger<TourOperatorController> logger)
	{
		_loaderService = loaderService;
		_logger = logger;
	}

	[HttpGet("Hotels")]
	public List<Hotel> GetAccommodations()
	{
		return FileUtils.GetData<List<Hotel>>("hotels.json");
	}

	[HttpPost("Hotels")]
	public async Task<bool> UpsertAccommodationsAsync([FromBody] List<HotelEntity> hotels)
	{
		await _loaderService.DeltaLoadAsync(hotels);
		return true;
	}

	[HttpGet("Transports")]
	public List<Transport> GetTransports()
	{
		return FileUtils.GetData<List<Transport>>("transports.json");
	}

	[HttpPost("Transports")]
	public async Task<bool> UpsertTransportsAsync([FromBody] List<TransportEntity> transports)
	{
		await _loaderService.DeltaLoadAsync(transports);
		return true;
	}

	[HttpGet("Tours")]
	public List<Tour> GetOffers()
	{
		return FileUtils.GetData<List<Tour>>("tours.json");
	}

	[HttpPost("Tours")]
	public async Task<bool> UpsertToursAsync([FromBody] List<TourEntity> tours)
	{
		await _loaderService.DeltaLoadAsync(tours);
		return true;
	}
}