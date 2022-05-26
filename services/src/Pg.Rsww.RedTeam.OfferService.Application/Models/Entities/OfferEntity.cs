using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.Common.Models.Offer;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class OfferEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	public string TourId { get; set; } = null!;

	public Reservation Reservation { get; set; } = null!;

	public People People { get; set; } = null!;

	public int GetTicketsCount() =>
		People.Adults + People.Teenagers + People.Children + People.Toddlers;
}