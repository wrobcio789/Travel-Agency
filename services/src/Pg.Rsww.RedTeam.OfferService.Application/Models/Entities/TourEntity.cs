using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class TourEntity : IEquatable<TourEntity>
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	public string Title { get; set; } = null!;

	public string Arrival { get; set; } = null!;

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public decimal Price { get; set; }

	#region Equatable

	public bool Equals(TourEntity? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Id == other.Id && Title == other.Title && Arrival == other.Arrival &&
		       StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate) && Price == other.Price;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((TourEntity)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Id, Title, Arrival, StartDate, EndDate, Price);
	}

	public static bool operator ==(TourEntity? left, TourEntity? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(TourEntity? left, TourEntity? right)
	{
		return !Equals(left, right);
	}

	#endregion Equatable
}