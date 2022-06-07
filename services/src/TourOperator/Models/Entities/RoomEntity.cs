using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.Common.Models.Offer;

namespace TourOperator.Models.Entities;

public class RoomEntity: IEquatable<RoomEntity>
{
	[BsonRepresentation(BsonType.String)]
	public RoomSize Size { get; set; }

	public int RoomCount { get; set; }

	public bool NewbornsFriendly { get; set; }

	[BsonElement("Amenities")]
	[JsonPropertyName("Amenities")]
	public List<string> Amenities { get; set; } = null!;

	#region Equality
	public bool Equals(RoomEntity? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Size == other.Size
		       && RoomCount == other.RoomCount
		       && NewbornsFriendly == other.NewbornsFriendly
		       && Amenities.SequenceEqual(other.Amenities);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((RoomEntity)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine((int)Size, RoomCount, NewbornsFriendly, Amenities);
	}

	public static bool operator ==(RoomEntity? left, RoomEntity? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(RoomEntity? left, RoomEntity? right)
	{
		return !Equals(left, right);
	}
	#endregion
}