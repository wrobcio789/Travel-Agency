using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.Common.Models.Offer;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class Reservation
{
	public DateTime StartDate { get; set; } 

	public DateTime EndDate { get; set; }
	
	public Accommodation Accommodation { get; set; } = null!;

	public string StartTransport { get; set; } = null!;

	public string EndTransport { get; set; } = null!;

	[BsonRepresentation(BsonType.String)]
	public ReservationStatus ReservationStatus { get; set; }
}