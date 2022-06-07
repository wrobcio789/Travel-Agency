using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace Pg.Rsww.RedTeam.DataStorage.Repositories;

public abstract class MongoChangeLoggingRepository<T> : MongoBaseRepository<T> where T : Entity
{
	private readonly ChangelogRepository _changelogRepository;


	protected MongoChangeLoggingRepository(IOptions<MongoSettings> mongoDBSettings, string collectionName)
		: base(mongoDBSettings, collectionName)
	{
		_changelogRepository = new ChangelogRepository(mongoDBSettings);
	}

	public override async Task AfterAction(ChangelogEntity changelogEntity)
	{
		if (changelogEntity == null)
		{
			return;
		}
		changelogEntity.Id = ObjectId.GenerateNewId().ToString();
		await _changelogRepository.CreateAsync(changelogEntity);
	}
}