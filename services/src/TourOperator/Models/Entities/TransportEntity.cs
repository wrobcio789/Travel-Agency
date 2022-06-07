using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace TourOperator.Models.Entities;

public class TransportEntity : Entity, IEquatable<TransportEntity>
{
	public string Arrival { get; set; } = null!;

	public string Departure { get; set; } = null!;

	[BsonRepresentation(BsonType.String)] public TransportType Type { get; set; }

	public int Capacity { get; set; } = 0;

	#region Equatable

	public bool Equals(TransportEntity? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Arrival == other.Arrival && Departure == other.Departure && Type == other.Type &&
		       Capacity == other.Capacity;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((TransportEntity)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Arrival, Departure, (int)Type, Capacity);
	}

	public static bool operator ==(TransportEntity? left, TransportEntity? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(TransportEntity? left, TransportEntity? right)
	{
		return !Equals(left, right);
	}

	#endregion Equatable
}