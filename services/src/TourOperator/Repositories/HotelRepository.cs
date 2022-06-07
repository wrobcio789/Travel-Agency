using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using TourOperator.Models.Entities;

namespace TourOperator.Repositories;

public class HotelRepository : MongoChangeLoggingRepository<HotelEntity>
{
	public HotelRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Hotels")
	{
	}
}