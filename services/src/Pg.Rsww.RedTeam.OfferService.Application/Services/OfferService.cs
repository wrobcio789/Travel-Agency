using AutoMapper;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.OfferService.Application.Models;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Services;

public class OfferService
{
	private readonly OfferRepository _offerDbService;
	private readonly TourRepository _tourDbService;
	private readonly HotelRepository _hotelDbService;
	private readonly TransportRepository _transportDbService;
	private readonly IMapper _mapper;

	public OfferService(
		OfferRepository offerDbService,
		TourRepository tourDbService,
		HotelRepository hotelDbService,
		TransportRepository transportDbService,
		IMapper mapper
	)
	{
		_offerDbService = offerDbService;
		_tourDbService = tourDbService;
		_hotelDbService = hotelDbService;
		_transportDbService = transportDbService;
		_mapper = mapper;
	}

	public async Task<OfferReservationResponse> MakeOfferAsync(OfferRequest offerRequest)
	{
		var offer = _mapper.Map<OfferEntity>(offerRequest);

		var tour = await _tourDbService.GetTourAsync(offer.TourId);
		if (tour == null)
		{
			return null;
		}
		
		offer.Reservation.ReservationStatus = ReservationStatus.Created;
		offer.Reservation.StartDate = tour.StartDate;
		offer.Reservation.EndDate = tour.EndDate;
		var isOfferAvailable = await IsOfferAvailableAsync(offerRequest);
		var id = "";
		if (isOfferAvailable.IsAvailable)
		{
			id = await _offerDbService.CreateAsync(offer);
		}

		var response = new OfferReservationResponse
		{
			OfferId = id,
			Price = isOfferAvailable.Price,
			IsReserved = isOfferAvailable.IsAvailable
		};

		return response; //TODO type
	}

	public async Task<OfferAvailabilityResponse> IsOfferAvailableAsync(OfferRequest offerRequest)
	{
		var offer = _mapper.Map<OfferEntity>(offerRequest);


		var tour = await _tourDbService.GetTourAsync(offer.TourId);
		if (tour == null)
		{
			return null;
		}


		var transportationToId = offerRequest.StartTransportId;
		var transportTo = await _transportDbService.GetTransport(transportationToId);
		if (transportTo == null)
		{
			return null;
		}

		var transportationFromId = offerRequest.EndTransportId;
		var transportFrom = await _transportDbService.GetTransport(transportationFromId);
		if (transportFrom == null)
		{
			return null;
		}

		var hotelId = offerRequest.Accommodation.HotelId;
		var hotel = await _hotelDbService.GetHotel(hotelId);
		if (hotel == null)
		{
			return null;
		}

		var isAvailable = true;
		
		var offersAccommodation = await _offerDbService.GetActiveOffersByAccommodation(
			tour.StartDate,
			tour.EndDate,
			hotelId
		);
		var isAccommodationAvailable = IsAccommodationAvailable(hotel, offer, offersAccommodation);
		isAvailable &= isAccommodationAvailable;

		var offersTransportTo = await _offerDbService.GetActiveOffersByStartTransport(
			tour.StartDate,
			transportationToId
		);
		var isTransportationFromAvailable = IsTransportAvailable(transportTo, offer, offersTransportTo);
		isAvailable &= isTransportationFromAvailable;
		var offersTransportFrom = await _offerDbService.GetActiveOffersByEndTransport(
			tour.EndDate,
			transportationFromId
		);

		var isTransportationToAvailable = IsTransportAvailable(transportFrom, offer, offersTransportFrom);
		isAvailable &= isTransportationToAvailable;


		decimal price = tour.Price * offer.GetTicketsCount() + offerRequest.Accommodation.NumberOfMeals * 20;
		if (offerRequest.PromoCode == "OFF10")
		{
			price *= 0.9M;
		}

		return new OfferAvailabilityResponse
		{
			IsAvailable = isAvailable,
			Price = price
		};
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
		var tours = await _tourDbService.GetAsync(
			departure,
			arrival,
			departureDate
		);
		return tours;
	}

	public async Task<List<HotelEntity>> GetHotelsAsync(string place)
	{
		var hotels = await _hotelDbService.GetAsync(null, null, place);
		return hotels;
	}

	public async Task<List<string>> GetDeparturesAsync()
	{
		var transports = await _transportDbService.GetAsync(null, null, null, null);
		var departures = transports
			.Where(x => !string.IsNullOrWhiteSpace(x.Departure))
			.GroupBy(x => x.Departure)
			.Select(g => g.First().Departure)
			.ToList();
		return departures;
	}
}