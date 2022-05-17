using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Pg.Rsww.RedTeam.DataStorage.Repositories;

public abstract class MongoBaseRepository<T>
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
	}

	public async Task CreateAsync(T entity)
	{
		await _collection.InsertOneAsync(entity);
	}

	public async Task<long> GetCountAsync()
	{
		return await _collection.CountDocumentsAsync(new BsonDocument());
	}

	public async Task DeleteAllAsync()
	{
		await _collection.DeleteManyAsync("{}");
	}
}