using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using TourOperator.Models.Entities;

namespace TourOperator.Repositories;

public class OfferRepository : MongoChangeLoggingRepository<OfferEntity>
{
	public OfferRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Offers")
	{
	}
}