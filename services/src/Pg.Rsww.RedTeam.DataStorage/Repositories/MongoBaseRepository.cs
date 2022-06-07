using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.DataStorage.Extensions;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace Pg.Rsww.RedTeam.DataStorage.Repositories;

public abstract class MongoBaseRepository<T> where T : Entity
{
	protected readonly IMongoCollection<T> _collection;

	protected MongoBaseRepository(IOptions<MongoSettings> mongoDBSettings, string collectionName)
	{
		MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);

		IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

		_collection = database.GetCollection<T>(collectionName);
	}

	public async Task CreateAsync(IList<T> entities)
	{
		await CreateAsync(entities, false);
	}

	public async Task CreateAsync(IList<T> entities, bool isFullLoad)
	{
		if (isFullLoad)
		{
			await _collection.DeleteManyAsync("{}");
		}

		await _collection.InsertManyAsync(entities);

		await AfterAction(new ChangelogEntity
		{
			DateTime = DateTime.Now,
			Database = _collection.CollectionNamespace.CollectionName,
			Old = null,
			New = JsonConvert.SerializeObject(entities, Formatting.Indented)
		});
	}

	public async Task CreateAsync(T entity)
	{
		await _collection.InsertOneAsync(entity);

		await AfterAction(new ChangelogEntity
		{
			DateTime = DateTime.Now,
			Database = _collection.CollectionNamespace.CollectionName,
			Old = null,
			New = JsonConvert.SerializeObject(entity, Formatting.Indented)
		});
	}

	public async Task<long> GetCountAsync()
	{
		return await _collection.CountDocumentsAsync(new BsonDocument());
	}

	public async Task DeleteAllAsync()
	{
		await _collection.DeleteManyAsync("{}");
		await AfterAction(new ChangelogEntity
		{
			DateTime = DateTime.Now,
			Database = _collection.CollectionNamespace.CollectionName,
			Old = "Remove all".SurroundWithQuotes(),
			New = "Empty".SurroundWithQuotes()
		});
	}

	public async Task UpdateAsync(T newEntity, T oldEntity)
	{
		var filter = Builders<T>.Filter.Eq(s => s.Id, newEntity.Id);
		var result = await _collection.ReplaceOneAsync(filter, newEntity);

		await AfterAction(new ChangelogEntity
		{
			DateTime = DateTime.Now,
			Database = _collection.CollectionNamespace.CollectionName,
			Old = JsonConvert.SerializeObject(oldEntity, Formatting.Indented),
			New = JsonConvert.SerializeObject(newEntity, Formatting.Indented)
		});
	}


	public async Task<List<T>> UpsertAsync(IList<T> elements)
	{
		var updateElements = new List<T>();
		foreach (var element in elements)
		{
			var tour = await GetAsync(element.Id);
			if (tour == null)
			{
				if (string.IsNullOrEmpty(element.Id))
				{
					element.Id = ObjectId.GenerateNewId().ToString();
				}

				await CreateAsync(element);
			}
			else if (!tour.Equals(element))
			{
				await UpdateAsync(element, tour);
			}
			updateElements.Add(element);
		}
		return updateElements;
	}

	public async Task<T> GetAsync(string id)
	{
		var builder = Builders<T>.Filter;
		var filter = builder.Empty;

		if (string.IsNullOrWhiteSpace(id))
		{
			return null;
		}

		var idFilter = builder.Eq(x => x.Id, id);
		filter &= idFilter;

		var elements = await _collection.Find(filter).ToListAsync();
		return elements.FirstOrDefault();
	}

	public async Task<List<T>> GetAllAsync()
	{
		return await _collection.Find(x => true).ToListAsync();
	}

	public async Task<List<T>> GetNewestAsync(int limit)
	{
		var sort = Builders<T>.Sort.Descending("_id");

		var filter1 = Builders<T>.Filter.Empty;

		var results = _collection.Find(filter1).Sort(sort);
		if (limit > 0)
		{
			results.Limit(limit);
		}

		return await results.ToListAsync();
	}

	public async Task<List<T>> GetRandomAsync(int count)
	{
		return await _collection.AsQueryable().Sample(count).ToListAsync();
	}

	public virtual async Task AfterAction(ChangelogEntity entity)
	{
	}
}