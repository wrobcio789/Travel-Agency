using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class TransportEntity : Entity
{
	public string Arrival { get; set; } = null!;

	public string Departure { get; set; } = null!;

	[BsonRepresentation(BsonType.String)]
	public TransportType Type { get; set; }

	public int Capacity { get; set; } = 0;
}