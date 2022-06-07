using Microsoft.AspNetCore.Mvc;
using TourOperator.Models.Api;
using TourOperator.Models.Entities;
using TourOperator.Repositories;
using TourOperator.Services;
using TourOperator.Utils;

namespace TourOperator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TourOperatorController : ControllerBase
{
	private readonly LoaderService _loaderService;
	private readonly HotelRepository _hotelRepository;
	private readonly TransportRepository _transportRepository;
	private readonly TourRepository _tourRepository;
	private readonly ILogger<TourOperatorController> _logger;

	public TourOperatorController(
		LoaderService loaderService,
		HotelRepository hotelRepository,
		TransportRepository transportRepository,
		TourRepository tourRepository,
		ILogger<TourOperatorController> logger)
	{
		_loaderService = loaderService;
		_hotelRepository = hotelRepository;
		_transportRepository = transportRepository;
		_tourRepository = tourRepository;
		_logger = logger;
	}

	[HttpGet("Hotels")]
	public async Task<List<HotelEntity>> GetAccommodationsAsync()
	{
		return await _hotelRepository.GetAllAsync();
	}

	[HttpPost("Hotels")]
	public async Task<bool> UpsertAccommodationsAsync([FromBody] List<HotelEntity> hotels)
	{
		await _loaderService.DeltaLoadAsync(hotels);
		return true;
	}

	[HttpGet("Transports")]
	public async Task<List<TransportEntity>> GetTransportsAsync()
	{
		return await _transportRepository.GetAllAsync();
	}

	[HttpPost("Transports")]
	public async Task<bool> UpsertTransportsAsync([FromBody] List<TransportEntity> transports)
	{
		await _loaderService.DeltaLoadAsync(transports);
		return true;
	}

	[HttpGet("Tours")]
	public async Task<List<TourEntity>> GetToursAsync()
	{
		return await _tourRepository.GetAllAsync();
	}

	[HttpPost("Tours")]
	public async Task<bool> UpsertToursAsync([FromBody] List<TourEntity> tours)
	{
		await _loaderService.DeltaLoadAsync(tours);
		return true;
	}
}