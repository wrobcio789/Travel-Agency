using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace Pg.Rsww.RedTeam.DataStorage.Repositories;

public class ChangelogRepository : MongoBaseRepository<ChangelogEntity>
{
	public ChangelogRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Changelog")
	{
	}
}