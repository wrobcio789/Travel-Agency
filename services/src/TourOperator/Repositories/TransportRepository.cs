using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using TourOperator.Models.Entities;

namespace TourOperator.Repositories;

public class TransportRepository : MongoChangeLoggingRepository<TransportEntity>
{
	public TransportRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Transports")
	{
	}
}