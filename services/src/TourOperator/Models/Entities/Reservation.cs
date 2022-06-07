using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.Common.Models.Offer;

namespace TourOperator.Models.Entities;

public class Reservation: IEquatable<Reservation>
{
	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }
	
	public Accommodation Accommodation { get; set; } = null!;

	public string StartTransport { get; set; } = null!;

	public string EndTransport { get; set; } = null!;

	[BsonRepresentation(BsonType.String)]
	public ReservationStatus ReservationStatus { get; set; }

	#region Equality
	public bool Equals(Reservation? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate) && Accommodation.Equals(other.Accommodation) && StartTransport == other.StartTransport && EndTransport == other.EndTransport && ReservationStatus == other.ReservationStatus;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Reservation)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(StartDate, EndDate, Accommodation, StartTransport, EndTransport, (int)ReservationStatus);
	}

	public static bool operator ==(Reservation? left, Reservation? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(Reservation? left, Reservation? right)
	{
		return !Equals(left, right);
	}
	#endregion
}