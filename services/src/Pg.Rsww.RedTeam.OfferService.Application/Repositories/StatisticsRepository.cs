using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OfferService.Application.Repositories;

public class StatisticsRepository : MongoBaseRepository<StatisticsEntity>
{

	public StatisticsRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Statistics")
	{
	}

	public async Task<List<StatisticsEntity>> GetStatistics(StatisticsDomains domain, int limit)
	{
		var sort = Builders<StatisticsEntity>.Sort.Descending(x => x.Count);

		var filter = Builders<StatisticsEntity>.Filter.Eq(x => x.Domain, domain);

		var results = _collection.Find(filter).Sort(sort);
		if (limit > 0)
		{
			results.Limit(limit);
		}

		return await results.ToListAsync();
	}

	public async Task Add(StatisticsDomains domain, string type, int amount)
	{
		await InsertOrIncrement(domain, type, amount);
	}

	private async Task<bool> InsertOrIncrement(StatisticsDomains domain, string name,int amount)
	{
		var builder = Builders<StatisticsEntity>.Filter;
		var filter = builder.Empty;

		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		var domainFilter = builder.Eq(x => x.Domain, domain);
		filter &= domainFilter;

		var nameFilter = builder.Eq(x => x.Name, name);
		filter &= nameFilter;

		var elements = await _collection.Find(filter).ToListAsync();
		if (!elements.Any())
		{
			await _collection.InsertOneAsync(new StatisticsEntity
			{
				Domain = domain,
				Name = name,
				Count = amount
			});
			return true;
		}

		var update = Builders<StatisticsEntity>
			.Update
			.Inc(x => x.Count, amount);

		var result = await _collection.UpdateOneAsync(filter, update);
		return result.IsModifiedCountAvailable && result.ModifiedCount == 1;
	}
}