using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using TourOperator.Models.Entities;

namespace TourOperator.Repositories;

public class TourRepository : MongoChangeLoggingRepository<TourEntity>
{
	public TourRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Tours")
	{
	}
}