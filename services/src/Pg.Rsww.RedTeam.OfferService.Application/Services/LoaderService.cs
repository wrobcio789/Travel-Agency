using AutoMapper;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Clients;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Services;

public class LoaderService
{
	private readonly TourRepository _tourRepository;
	private readonly HotelRepository _hotelRepository;
	private readonly TransportRepository _transportRepository;
	private readonly IMapper _mapper;

	private readonly TourOperatorClient _client;

	public LoaderService(
		TourRepository tourRepository,
		HotelRepository hotelRepository,
		TransportRepository transportRepository,
		IMapper mapper, TourOperatorClient client
	)
	{
		_tourRepository = tourRepository;
		_hotelRepository = hotelRepository;
		_transportRepository = transportRepository;
		_mapper = mapper;
		_client = client;
	}

	public async Task<bool> FullLoadAsync()
	{
		return await FullLoadAsync(true);
	}

	public async Task<bool> FullLoadAsync(bool overrideData)
	{
		var existingHotelCount = await _hotelRepository.GetCountAsync();
		var existingTransportCount = await _transportRepository.GetCountAsync();
		var existingTourCount = await _tourRepository.GetCountAsync();

		List<HotelResponse> hotelsResponse = null;
		List<TransportResponse> transportsResponse = null;
		List<TourResponse> toursResponses = null;

		if (overrideData || existingHotelCount == 0)
		{
			hotelsResponse = await _client.GetAsync<List<HotelResponse>>("Hotels");
		}

		if (overrideData || existingTransportCount == 0)
		{
			transportsResponse =
				await _client.GetAsync<List<TransportResponse>>("Transports");
		}

		if (overrideData || existingTourCount == 0)
		{
			toursResponses =
				await _client.GetAsync<List<TourResponse>>("Tours");
		}


		var areAllLoaded = true;
		if (hotelsResponse != null)
		{
			var hotels = _mapper.Map<List<HotelEntity>>(hotelsResponse).Distinct().ToList();

			await InsertAsync(overrideData, hotels, _hotelRepository);
		}
		else
		{
			areAllLoaded = false;
		}

		if (transportsResponse != null)
		{
			var transports = _mapper.Map<List<TransportEntity>>(transportsResponse).Distinct().ToList();

			await InsertAsync(overrideData, transports, _transportRepository);
		}
		else
		{
			areAllLoaded = false;
		}

		if (toursResponses != null)
		{
			var tours = _mapper.Map<List<TourEntity>>(toursResponses);
			var distinctTours = tours.Distinct().ToList();

			await InsertAsync(overrideData, distinctTours, _tourRepository);
		}
		else
		{
			areAllLoaded = false;
		}

		return areAllLoaded || !overrideData;
	}


	private async Task InsertAsync<T>(bool overrideData, IList<T> elements, MongoBaseRepository<T> repository)
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