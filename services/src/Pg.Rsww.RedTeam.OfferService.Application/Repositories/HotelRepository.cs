using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OfferService.Application.Repositories;

public class HotelRepository : MongoBaseRepository<HotelEntity>
{
	public HotelRepository(IOptions<MongoSettings> mongoDBSettings) : base(mongoDBSettings, "Hotels")
	{
	}

	public async Task<List<HotelEntity>> GetAsync(string? country, string? city, string? region)
	{
		var builder = Builders<HotelEntity>.Filter;
		var filter = builder.Empty;

		if (!string.IsNullOrWhiteSpace(country))
		{
			var countryFilter = builder.Eq(x => x.Country, country);
			filter &= countryFilter;
		}

		if (!string.IsNullOrWhiteSpace(city))
		{
			var cityFilter = builder.Eq(x => x.City, city);
			filter &= cityFilter;
		}

		if (!string.IsNullOrWhiteSpace(region))
		{
			var regionFilter = builder.Eq(x => x.Region, region);
			filter &= regionFilter;
		}

		return await _collection.Find(filter).ToListAsync();
	}

	public async Task<HotelEntity> GetHotel(string hotelId)
	{
		if (string.IsNullOrWhiteSpace(hotelId))
		{
			return null;
		}
		var builder = Builders<HotelEntity>.Filter;
		var countryFilter = builder.Eq(x => x.Id, hotelId);

		return await _collection
			.Find(countryFilter)
			.FirstOrDefaultAsync();
	}
}