using AutoMapper;
using Pg.Rsww.RedTeam.DataStorage.Models;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using TourOperator.ExternalServices.OfferService.Clients;
using TourOperator.Models.Api;
using TourOperator.Models.Entities;
using TourOperator.Repositories;
using TourOperator.Utils;

namespace TourOperator.Services;

public class LoaderService
{
	private readonly TourRepository _tourRepository;
	private readonly HotelRepository _hotelRepository;
	private readonly TransportRepository _transportRepository;
	private readonly OfferServiceClient _client;
	private readonly IMapper _mapper;

	public LoaderService(
		TourRepository tourRepository,
		HotelRepository hotelRepository,
		TransportRepository transportRepository,
		OfferServiceClient client,
		IMapper mapper
	)
	{
		_tourRepository = tourRepository;
		_hotelRepository = hotelRepository;
		_transportRepository = transportRepository;
		_client = client;
		_mapper = mapper;
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

		List<Hotel> hotelsResponse = null;
		List<Transport> transportsResponse = null;
		List<Tour> toursResponses = null;

		if (overrideData || existingHotelCount == 0)
		{
			hotelsResponse = FileUtils.GetData<List<Hotel>>("hotels.json");
		}

		if (overrideData || existingTransportCount == 0)
		{
			transportsResponse = FileUtils.GetData<List<Transport>>("transports.json");
		}

		if (overrideData || existingTourCount == 0)
		{
			toursResponses = FileUtils.GetData<List<Tour>>("tours.json");
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


	private async Task InsertAsync<T>(bool overrideData, IList<T> elements, MongoChangeLoggingRepository<T> repository) where T : Entity
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

	public async Task DeltaLoadAsync(IList<TourEntity> elements)
	{
		var records = await _tourRepository.UpsertAsync(elements);
		if (records.Any())
		{
			await _client.PostAsync("TourDelta", records);
		}
	}
	public async Task DeltaLoadAsync(IList<TransportEntity> elements)
	{
		var records = await _transportRepository.UpsertAsync(elements);
		if (records.Any())
		{
			await _client.PostAsync("TransportDelta", records);
		}
	}
	public async Task DeltaLoadAsync(IList<HotelEntity> elements)
	{
		var records = await _hotelRepository.UpsertAsync(elements);
		if (records.Any())
		{
			await _client.PostAsync("HotelsDelta", records);
		}
	}
}