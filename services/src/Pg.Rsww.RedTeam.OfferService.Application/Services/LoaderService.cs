using AutoMapper;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Clients;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Services;

public class LoaderService
{
	private readonly TourRepository _tourDbService;
	private readonly HotelRepository _hotelDbService;
	private readonly TransportRepository _transportDbService;
	private readonly IMapper _mapper;

	private readonly TourOperatorClient _client;

	public LoaderService(
		TourRepository tourDbService,
		HotelRepository hotelDbService,
		TransportRepository transportDbService,
		IMapper mapper, TourOperatorClient client
	)
	{
		_tourDbService = tourDbService;
		_hotelDbService = hotelDbService;
		_transportDbService = transportDbService;
		_mapper = mapper;
		_client = client;
	}

	public async Task<bool> FullLoadAsync()
	{
		return await FullLoadAsync(true);
	}

	public async Task<bool> FullLoadAsync(bool overrideData)
	{
		var hotelsResponse = await _client.GetAsync<List<HotelResponse>>("Hotels");
		var transportsResponse =
			await _client.GetAsync<List<TransportResponse>>("Transports");
		var toursResponses =
			await _client.GetAsync<List<TourResponse>>("Tours");

		var areAllLoaded = true;
		if (hotelsResponse != null)
		{
			var hotels = _mapper.Map<List<HotelEntity>>(hotelsResponse).Distinct().ToList();

			await InsertAsync(overrideData, hotels, _hotelDbService);
		}
		else
		{
			areAllLoaded = false;
		}

		if (transportsResponse != null)
		{
			var transports = _mapper.Map<List<TransportEntity>>(transportsResponse).Distinct().ToList();
			
			await InsertAsync(overrideData, transports, _transportDbService);
		}
		else
		{
			areAllLoaded = false;
		}

		if (toursResponses != null)
		{
			var tours = _mapper.Map<List<TourEntity>>(toursResponses);
			var distinctTours = tours.Distinct().ToList();
			
			await InsertAsync(overrideData, distinctTours, _tourDbService);
		}
		else
		{
			areAllLoaded = false;
		}

		return areAllLoaded;
	}


	private async Task InsertAsync<T>(bool overrideData, IList<T> elements,MongoBaseRepository<T> repository)
	{
		var isEmpty = true;
		if (!overrideData)
		{
			var existingCount = await repository.GetCountAsync();
			isEmpty = existingCount == 0;
		}

		if (overrideData || isEmpty)
		{
			await repository.CreateAsync(elements, true);
		}
	}
}