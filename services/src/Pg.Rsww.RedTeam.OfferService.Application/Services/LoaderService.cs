using Pg.Rsww.RedTeam.DataStorage.Models;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Clients;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Services;

public class LoaderService
{
	private readonly TourRepository _tourRepository;
	private readonly HotelRepository _hotelRepository;
	private readonly TransportRepository _transportRepository;

	private readonly TourOperatorClient _client;

	public LoaderService(
		TourRepository tourRepository,
		HotelRepository hotelRepository,
		TransportRepository transportRepository,
		TourOperatorClient client
	)
	{
		_tourRepository = tourRepository;
		_hotelRepository = hotelRepository;
		_transportRepository = transportRepository;
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

		List<HotelEntity> hotels = null;
		List<TransportEntity> transports = null;
		List<TourEntity> tours = null;

		if (overrideData || existingHotelCount == 0)
		{
			hotels = await _client.GetAsync<List<HotelEntity>>("Hotels");
		}

		if (overrideData || existingTransportCount == 0)
		{
			transports =
				await _client.GetAsync<List<TransportEntity>>("Transports");
		}

		if (overrideData || existingTourCount == 0)
		{
			tours = await _client.GetAsync<List<TourEntity>>("Tours");
		}


		var areAllLoaded = true;
		if (hotels != null && hotels.Any())
		{
			await InsertAsync(overrideData, hotels, _hotelRepository);
		}
		else
		{
			areAllLoaded = false;
		}

		if (transports != null && transports.Any())
		{
			await InsertAsync(overrideData, transports, _transportRepository);
		}
		else
		{
			areAllLoaded = false;
		}

		if (tours != null && tours.Any())
		{
			var distinctTours = tours.Distinct().ToList();

			await InsertAsync(overrideData, distinctTours, _tourRepository);
		}
		else
		{
			areAllLoaded = false;
		}

		return areAllLoaded || !overrideData;
	}


	private async Task InsertAsync<T>(bool overrideData, IList<T> elements, MongoBaseRepository<T> repository) where T:Entity
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

	public async Task<bool> LoadDelta(List<TourEntity> tours)
	{
		if (tours == null) return false;
		

		await _tourRepository.UpsertAsync(tours);
		return true;
	}

	public async Task<bool> LoadDelta(List<HotelEntity> hotels)
	{
		if (hotels == null) return false;

		await _hotelRepository.UpsertAsync(hotels);
		return true;
	}

	public async Task<bool> LoadDelta(List<TransportEntity> transport)
	{
		if (transport == null) return false;

		await _transportRepository.UpsertAsync(transport);
		return true;
	}
}