using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OfferService.Application.Repositories;

public class TourRepository : MongoBaseRepository<TourEntity>
{
	public TourRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Tours")
	{
	}

	public async Task<List<TourEntity>> GetAsync(string? departure, string? arrival, DateTime departureDate)
	{
		return await GetAsync(departure, arrival, departureDate, true);
	}

	public async Task<List<TourEntity>> GetAsync(string? departure, string? arrival, DateTime departureDate, bool enabled)
	{
		var builder = Builders<TourEntity>.Filter;
		var filter = builder.Empty;

		if (!string.IsNullOrWhiteSpace(arrival))
		{
			var arrivalFilter = builder.Eq(x => x.Arrival, arrival);
			filter &= arrivalFilter;
		}

		if (departureDate != null)
		{
			var departureDateFilter = builder.Gte(x => x.StartDate, departureDate);
			filter &= departureDateFilter;
		}

		var enabledFilter = builder.Eq(x => x.Enabled, enabled);
		filter &= enabledFilter;

		return await _collection.Find(filter).ToListAsync();
	}

	public async Task<TourEntity> GetTourAsync(string tourId)
	{
		var builder = Builders<TourEntity>.Filter;
		var filter = builder.Empty;
		
		if (string.IsNullOrWhiteSpace(tourId))
		{
			return null;
		}
		var arrivalFilter = builder.Eq(x => x.Id, tourId);
		filter &= arrivalFilter;
		var tours = await _collection.Find(filter).ToListAsync();
		return tours.FirstOrDefault();
	}
}