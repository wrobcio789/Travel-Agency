using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class StatisticsEntity : Entity
{
	[BsonRepresentation(BsonType.String)]
	public StatisticsDomains Domain { get; set; }
	public string Name { get; set; }
	public int Count { get; set; }
}
