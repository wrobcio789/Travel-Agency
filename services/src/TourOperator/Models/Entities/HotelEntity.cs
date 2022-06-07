using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace TourOperator.Models.Entities;

public class HotelEntity : Entity, IEquatable<HotelEntity>
{
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

	#region Equality

	public bool Equals(HotelEntity? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Country == other.Country && City == other.City && Region == other.Region && Name == other.Name &&
		       Rooms.SequenceEqual(other.Rooms) && Amenities.SequenceEqual(other.Amenities);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((HotelEntity)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Country, City, Region, Name, Rooms, Amenities);
	}

	public static bool operator ==(HotelEntity? left, HotelEntity? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(HotelEntity? left, HotelEntity? right)
	{
		return !Equals(left, right);
	}

	#endregion
}