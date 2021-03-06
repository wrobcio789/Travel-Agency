using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.Common.Models.Offer.Simple;
using Pg.Rsww.RedTeam.OfferService.Api.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Services;

namespace Pg.Rsww.RedTeam.OfferService.Api.Controllers;

[Controller]
[Route("api/[controller]")]
public class OffersController : Controller
{
	private readonly Application.Services.OfferService _offerService;
	private readonly IMapper _mapper;

	public OffersController(
		Application.Services.OfferService offerService,
		IMapper mapper
	)
	{
		_offerService = offerService;
		_mapper = mapper;
	}

	/// <summary>
	/// Endpoint for searching offers
	/// </summary>
	/// <param name="tourRequest"></param>
	/// <returns></returns>
	[HttpPost("Search")]
	public async Task<List<TourSearchResponse>> PostSearch([FromBody] TourRequest tourRequest)
	{
		var tours = await _offerService.SearchToursAsync(
			tourRequest.Departure,
			tourRequest.Arrival,
			tourRequest.DepartureDate
		);

		var response = _mapper.Map<List<TourSearchResponse>>(tours);

		return response;
	}

	/// <summary>
	/// Endpoint for checking if offer is available
	/// </summary>
	/// <param name="simpleOfferRequest"></param>
	/// <returns></returns>
	[HttpPost("Availability")]
	public async Task<OfferAvailabilityResponse> PostAvailability([FromBody] SimpleOfferRequest simpleOfferRequest)
	{
		var offerRequest = _mapper.Map<OfferRequest>(simpleOfferRequest);
		var result = await _offerService.IsOfferAvailableAsync(offerRequest);
		return result;
	}

	/// <summary>
	/// Endpoint for listing hotels with amenities
	/// </summary>
	/// <param name="place"></param>
	/// <returns></returns>
	[HttpGet("Hotels")]
	public async Task<List<HotelListingResponse>> GetHotels(string place)
	{
		var hotels = await _offerService.GetHotelsAsync(place);
		var response = _mapper.Map<List<HotelListingResponse>>(hotels);

		return response;
	}

	/// <summary>
	/// Endpoint for listing departure locations for transportation
	/// </summary>
	/// <returns></returns>
	[HttpGet("Transport/Departure")]
	public async Task<List<string>> GetDepartures()
	{
		var departures = await _offerService.GetDeparturesAsync();
		return departures;
	}

	[HttpGet("Statistics")]
	public async Task<StatisticsAggregate> GetDepartures(int head)
	{
		return await _offerService.GetTopStatistics(head);
	}
}