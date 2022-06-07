using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Pg.Rsww.RedTeam.Common.Models.Offer;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class RoomEntity
{

	[BsonRepresentation(BsonType.String)]
	public RoomSize Size { get; set; }

	public int RoomCount { get; set; }

	public bool NewbornsFriendly { get; set; }

	[BsonElement("Amenities")]
	[JsonPropertyName("Amenities")]
	public List<string> Amenities { get; set; } = null!;
}