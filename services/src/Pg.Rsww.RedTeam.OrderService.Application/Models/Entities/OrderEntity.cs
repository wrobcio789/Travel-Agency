using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.Common.Models;

namespace Pg.Rsww.RedTeam.OrderService.Application.Models.Entities;

public class OrderEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	
	public string CustomerId { get; set; }

	public string OfferId { get; set; }

	public string PaymentId { get; set; }

	public DateTime CreatedAt { get; set; }

	[BsonRepresentation(BsonType.String)]
	public ReservationStatus Status { get; set; }
}