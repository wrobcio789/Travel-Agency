using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using ReservationStatus = Pg.Rsww.RedTeam.Common.Models.ReservationStatus;

namespace Pg.Rsww.RedTeam.OfferService.Application.Repositories;

public class OfferRepository : MongoBaseRepository<OfferEntity>
{
	public OfferRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Offers")
	{
	}

	public new async Task<string> CreateAsync(OfferEntity offer)
	{
		offer.Id = ObjectId.GenerateNewId().ToString();
		await _collection.InsertOneAsync(offer);
		return offer.Id;
	}

	public async Task<long> UpdateStatus(List<string> offerIds, ReservationStatus status)
	{
		var builder = Builders<OfferEntity>.Filter;
		var offerIdFilter = builder.In(x => x.Id, offerIds);

		var update = Builders<OfferEntity>
			.Update
			.Set(x => x.Reservation.ReservationStatus, status);

		var result = await _collection.UpdateManyAsync(offerIdFilter, update);
		return result.IsModifiedCountAvailable ? result.ModifiedCount : 0;
	}

	public async Task<long> UpdateStatus(string offerId, ReservationStatus status)
	{
		return await UpdateStatus(new List<string> { offerId }, status);
	}

	public async Task<List<OfferEntity>> GetActiveOffersByAccommodation(
		DateTime startDate,
		DateTime endDate,
		string hotelId
	)
	{
		var filterBuilder = Builders<OfferEntity>.Filter;
		var filter = filterBuilder.Empty;

		var statusFilter = filterBuilder.Ne(x => x.Reservation.ReservationStatus, ReservationStatus.Cancelled);
		filter &= statusFilter;

		var accommodationFilter = filterBuilder.Eq(x => x.Reservation.Accommodation.HotelId, hotelId);
		filter &= accommodationFilter;

		var offers = await _collection.Find(filter).ToListAsync();
		return offers.Where(x => x.Reservation.StartDate < endDate && startDate < x.Reservation.EndDate).ToList();
	}


	public async Task<List<OfferEntity>> GetActiveOffersByStartTransport(
		DateTime date,
		string transportId
	)
	{
		var startDate = GetStartDate(date);
		var endDate = GetEndDate(date);

		var filterBuilder = Builders<OfferEntity>.Filter;
		var filter = filterBuilder.Empty;

		var statusFilter = filterBuilder.Ne(x => x.Reservation.ReservationStatus, ReservationStatus.Cancelled);
		filter &= statusFilter;

		var transportationToFilter = filterBuilder.Eq(x => x.Reservation.StartTransport, transportId);
		filter &= transportationToFilter;

		var offers = await _collection.Find(filter).ToListAsync();
		return offers.Where(x => x.Reservation.StartDate < endDate && startDate < x.Reservation.StartDate).ToList();
	}

	public async Task<List<OfferEntity>> GetActiveOffersByEndTransport(
		DateTime date,
		string transportId
	)
	{
		var startDate = GetStartDate(date);
		var endDate = GetEndDate(date);

		var filterBuilder = Builders<OfferEntity>.Filter;
		var filter = filterBuilder.Empty;

		var statusFilter = filterBuilder.Ne(x => x.Reservation.ReservationStatus, ReservationStatus.Cancelled);
		filter &= statusFilter;

		var transportationFromFilter = filterBuilder.Eq(x => x.Reservation.EndTransport, transportId);
		filter &= transportationFromFilter;

		var offers = await _collection.Find(filter).ToListAsync();
		return offers.Where(x => x.Reservation.EndDate < endDate && startDate < x.Reservation.EndDate).ToList();
	}

	private DateTime GetStartDate(DateTime date)
	{
		return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
	}

	private DateTime GetEndDate(DateTime date)
	{
		return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
	}
}