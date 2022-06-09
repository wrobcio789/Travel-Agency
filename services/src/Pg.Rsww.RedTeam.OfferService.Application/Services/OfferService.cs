using AutoMapper;
using Microsoft.Extensions.Logging;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.OfferService.Application.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Services;

public class OfferService
{
	private readonly OfferRepository _offerRepository;
	private readonly TourRepository _tourRepository;
	private readonly HotelRepository _hotelRepository;
	private readonly TransportRepository _transportRepository;
	private readonly StatisticsRepository _statisticsRepository;
	private readonly IMapper _mapper;
	private readonly ILogger<OfferService> _logger;

	public OfferService(
		OfferRepository offerRepository,
		TourRepository tourRepository,
		HotelRepository hotelRepository,
		TransportRepository transportRepository,
		StatisticsRepository statisticsRepository,
		IMapper mapper,
		ILogger<OfferService> logger
	)
	{
		_offerRepository = offerRepository;
		_tourRepository = tourRepository;
		_hotelRepository = hotelRepository;
		_transportRepository = transportRepository;
		_statisticsRepository = statisticsRepository;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<OfferReservationResponse> MakeOfferAsync(OfferRequest offerRequest)
	{
		var tour = await _tourRepository.GetTourAsync(offerRequest.TourId);
		if (tour == null)
		{
			_logger.Log(LogLevel.Warning, $"Tour could not be found, id:{offerRequest.TourId}");
			return null;
		}

		var offerAvailabilityResponse = await IsOfferAvailableAsync(offerRequest);
		var offer = _mapper.Map<OfferEntity>(offerRequest);
		offer.Reservation.ReservationStatus = ReservationStatus.Created;
		offer.Reservation.StartDate = tour.StartDate;
		offer.Reservation.EndDate = tour.EndDate;
		var id = "";
		if (offerAvailabilityResponse.IsAvailable)
		{
			id = await _offerRepository.CreateAsync(offer);
			_logger.Log(LogLevel.Information, $"Offer is available id:{id}");
		}

		var response = new OfferReservationResponse
		{
			OfferId = id,
			Price = offerAvailabilityResponse.Price,
			IsReserved = offerAvailabilityResponse.IsAvailable
		};

		return response;
	}

	public async Task<OfferAvailabilityResponse> IsOfferAvailableAsync(OfferRequest offerRequest)
	{
		var offer = _mapper.Map<OfferEntity>(offerRequest);
		if (offer == null)
		{
			_logger.Log(LogLevel.Error, "Could not map offer request to offer");
			return null;
		}

		var tour = await _tourRepository.GetTourAsync(offer.TourId);
		if (tour == null)
		{
			_logger.Log(LogLevel.Warning, $"Tour could not be found, id:{offerRequest.TourId}");
			return null;
		}

		var departureCity = offerRequest.TransportationTo.Departure;
		var arrivalCity = tour.City;

		var transportTo = await _transportRepository.GetSingleAsync(
			departureCity,
			arrivalCity,
			offerRequest.TransportationTo.Type
		);
		if (transportTo == null)
		{
			_logger.Log(LogLevel.Warning, "TransportTo could not be found");
			return null;
		}

		offerRequest.StartTransportId = transportTo.Id;

		var transportFrom = await _transportRepository.GetSingleAsync(
			arrivalCity,
			departureCity,
			offerRequest.TransportationFrom.Type
		);
		if (transportFrom == null)
		{
			_logger.Log(LogLevel.Warning, "TransportFrom could not be found");
			return null;
		}

		offerRequest.EndTransportId = transportFrom.Id;

		var hotelId = offerRequest.Accommodation.HotelId;
		var hotel = await _hotelRepository.GetHotel(hotelId);
		if (hotel == null)
		{
			_logger.Log(LogLevel.Warning, "Hotel could not be found, id:{hotelId}", hotelId);
			return null;
		}

		var isAvailable = true;

		var offersAccommodation = await _offerRepository.GetActiveOffersByAccommodation(
			tour.StartDate,
			tour.EndDate,
			hotelId
		);
		var isAccommodationAvailable = IsAccommodationAvailable(hotel, offer, offersAccommodation);
		if (!isAccommodationAvailable) _logger.Log(LogLevel.Information, "Accommodation is not available");
		isAvailable &= isAccommodationAvailable;

		var offersTransportTo = await _offerRepository.GetActiveOffersByStartTransport(
			tour.StartDate,
			offerRequest.StartTransportId
		);
		var isTransportationFromAvailable = IsTransportAvailable(transportTo, offer, offersTransportTo);
		if (!isTransportationFromAvailable) _logger.Log(LogLevel.Information, "TransportationFrom is not available");
		isAvailable &= isTransportationFromAvailable;
		var offersTransportFrom = await _offerRepository.GetActiveOffersByEndTransport(
			tour.EndDate,
			offerRequest.EndTransportId
		);

		var isTransportationToAvailable = IsTransportAvailable(transportFrom, offer, offersTransportFrom);
		if (!isTransportationToAvailable) _logger.Log(LogLevel.Information, "TransportationTo is not available");
		isAvailable &= isTransportationToAvailable;


		var price = CalculatePrice(offerRequest, tour, offer);

		return new OfferAvailabilityResponse
		{
			IsAvailable = isAvailable,
			Price = price
		};
	}

	private static decimal CalculatePrice(OfferRequest offerRequest, TourEntity tour, OfferEntity offer)
	{
		decimal price = tour.Price * offer.GetTicketsCount() + offerRequest.Accommodation.NumberOfMeals * 20;
		if (offerRequest.PromoCode == "OFF10")
		{
			price *= 0.9M;
		}

		return price;
	}

	private bool IsAccommodationAvailable(
		HotelEntity hotelEntity,
		OfferEntity offer,
		List<OfferEntity> offersAccommodation
	)
	{
		var roomNeeded = new Dictionary<RoomSize, int>();
		foreach (var roomSize in (RoomSize[])Enum.GetValues(typeof(RoomSize)))
		{
			var count = offer.Reservation.Accommodation.Rooms.Where(x => x.RoomSize == roomSize)
				.Sum(x => x.Count);
			if (count > 0)
			{
				roomNeeded[roomSize] = count;
			}
		}

		var roomCapacity = new Dictionary<RoomSize, int>();

		foreach (var roomSize in roomNeeded.Keys)
		{
			roomCapacity[roomSize] = hotelEntity.Rooms
				.Where(x => x.Size == roomSize)
				.Select(x => x.RoomCount)
				.FirstOrDefault();
		}

		foreach (var offerEntity in offersAccommodation)
		{
			foreach (var roomSize in roomNeeded.Keys)
			{
				roomCapacity[roomSize] -= offerEntity.Reservation.Accommodation.Rooms
					.Where(x => x.RoomSize == roomSize).Sum(x => x.Count);
			}
		}

		foreach (var roomSize in roomNeeded.Keys)
		{
			if (roomNeeded[roomSize] > 0 && roomCapacity[roomSize] - roomNeeded[roomSize] < 0)
			{
				return false;
			}
		}

		return true;
	}

	private bool IsTransportAvailable(
		TransportEntity transportEntity,
		OfferEntity offer,
		List<OfferEntity> offersTransport
	)
	{
		if (transportEntity.Type == TransportType.Own)
		{
			return true;
		}

		var transportNeeded = offer.GetTicketsCount();
		var transportTaken = offersTransport.Sum(x => x.GetTicketsCount());
		var transportCapacity = transportEntity.Capacity;
		var transportAvailable = transportCapacity - transportTaken;
		return transportNeeded == 0 || transportAvailable - transportNeeded >= 0;
	}

	public async Task<List<TourEntity>> SearchToursAsync(string departure, string arrival, DateTime departureDate)
	{
		var tours = await _tourRepository.GetAsync(
			departure,
			arrival,
			departureDate
		);
		return tours;
	}

	public async Task<List<HotelEntity>> GetHotelsAsync(string place)
	{
		var hotels = await _hotelRepository.GetAsync(null, null, place);
		return hotels;
	}

	public async Task<List<string>> GetDeparturesAsync()
	{
		var transports = await _transportRepository.GetAsync(null, null, null, null);
		var departures = transports
			.Where(x => !string.IsNullOrWhiteSpace(x.Departure))
			.GroupBy(x => x.Departure)
			.Select(g => g.First().Departure)
			.ToList();
		return departures;
	}

	public async Task<StatisticsAggregate> GetTopStatistics(int head)
	{
		var transport = await _statisticsRepository.GetStatistics(StatisticsDomains.Transport, head);
		var hotels = await _statisticsRepository.GetStatistics(StatisticsDomains.Hotel, head);
		var tours = await _statisticsRepository.GetStatistics(StatisticsDomains.Tour, head);
		return new StatisticsAggregate
		{
			Transports = transport.Select(x => new TransportStatistics { Code = x.Name.ToUpper(), Visits = x.Count }).ToList(),
			Hotels = hotels.Select(x => new HotelStatistics() { HotelName = x.Name, Visits = x.Count }).ToList(),
			Tours = tours.Select(x => new TourPlaceStatistics() { Departure = x.Name, Visits = x.Count }).ToList()
		};
	}
}