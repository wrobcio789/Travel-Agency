using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OfferService.Application.Repositories;

public class TransportRepository : MongoBaseRepository<TransportEntity>
{
	public TransportRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Transports")
	{
	}

	public async Task<List<TransportEntity>> GetAsync(string? departure, string? arrival, TransportType? type,
		DateTime? date)
	{
		var builder = Builders<TransportEntity>.Filter;
		var filter = builder.Empty;

		if (!string.IsNullOrWhiteSpace(departure))
		{
			var countryFilter = builder.Eq(x => x.Departure, departure);
			filter &= countryFilter;
		}

		if (!string.IsNullOrWhiteSpace(arrival))
		{
			var countryFilter = builder.Eq(x => x.Arrival, arrival);
			filter &= countryFilter;
		}

		if (type.HasValue)
		{
			var countryFilter = builder.Eq(x => x.Type, type);
			filter &= countryFilter;
		}

		return await _collection.Find(filter).ToListAsync();
	}

	public async Task<TransportEntity> GetTransport(string transportId)
	{
		var builder = Builders<TransportEntity>.Filter;
		var filter = builder.Empty;

		if (!string.IsNullOrWhiteSpace(transportId))
		{
			var startTransportFilter = builder.Eq(x => x.Id, transportId);
			filter &= startTransportFilter;
		}

		var transports = await _collection.Find(filter).ToListAsync();
		return transports.FirstOrDefault();
	}

	public async Task<TransportEntity> GetSingleAsync(string departure, string arrival, TransportType type)
	{
		var builder = Builders<TransportEntity>.Filter;
		var filter = builder.Empty;

		if (type!=TransportType.Own)
		{
			if (!string.IsNullOrWhiteSpace(departure))
			{
				var departureFilter = builder.Eq(x => x.Departure, departure);
				filter &= departureFilter;
			}

			if (!string.IsNullOrWhiteSpace(arrival))
			{
				var departureFilter = builder.Eq(x => x.Arrival, arrival);
				filter &= departureFilter;
			}
		}

		var typeFilter = builder.Eq(x => x.Type, type);
		filter &= typeFilter;

		var transports = await _collection.Find(filter).ToListAsync();
		return transports.FirstOrDefault();
	}
}