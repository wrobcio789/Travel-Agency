using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.Common.Models.Offer.Request;
using Pg.Rsww.RedTeam.OfferService.Api.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models;

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

	[HttpPost("Search")]
	public async Task<List<TourResponse>> PostSearch([FromBody] TourRequest tourRequest)
	{
		var tours = await _offerService.SearchToursAsync(
			tourRequest.Departure,
			tourRequest.Arrival,
			tourRequest.DepartureDate
		);

		var response = _mapper.Map<List<TourResponse>>(tours);

		return response;
	}

	[HttpPost("Availability")]
	public async Task<OfferAvailabilityResponse> PostAvailability([FromBody] SimpleOfferRequest simpleOfferRequest)
	{
		var offerRequest = _mapper.Map<OfferRequest>(simpleOfferRequest);
		var result = await _offerService.IsOfferAvailableAsync(offerRequest);
		if (result == null)
		{
			return new OfferAvailabilityResponse
			{
				IsAvailable = false,
				Price = 0.00m,
			};
		}
		return result;
	}


	[HttpGet("Hotels")]
	public async Task<List<HotelListingResponse>> GetHotels(string place)
	{
		var hotels = await _offerService.GetHotelsAsync(place);
		var response = _mapper.Map<List<HotelListingResponse>>(hotels);

		return response;
	}

	[HttpGet("Transport/Departure")]
	public async Task<List<string>> GetDepartures()
	{
		var departures = await _offerService.GetDeparturesAsync();
		return departures;
	}
}