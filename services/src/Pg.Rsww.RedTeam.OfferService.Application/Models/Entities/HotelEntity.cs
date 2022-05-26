using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class HotelEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	public string Country { get; set; } = null!;

	public string City { get; set; } = null!;

	public string Region { get; set; } = null!;

	public string Name { get; set; } = null!;

	[BsonElement("Rooms")]
	[JsonPropertyName("Rooms")]
	public List<RoomEntity> Rooms { get; set; } = null!;
	[BsonElement("Amenities")]
	[JsonPropertyName("Amenities")]
	public List<string> Amenities { get; set; } = null!;
}