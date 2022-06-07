using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace TourOperator.Models.Entities;

public class OfferEntity : Entity, IEquatable<OfferEntity>
{
	public string TourId { get; set; } = null!;

	public Reservation Reservation { get; set; } = null!;

	public People People { get; set; } = null!;

	public int GetTicketsCount() =>
		People.Adults + People.Teenagers + People.Children + People.Toddlers;

	#region Equality

	public bool Equals(OfferEntity? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return TourId == other.TourId && Reservation.Equals(other.Reservation) && People.Equals(other.People);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((OfferEntity)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(TourId, Reservation, People);
	}

	public static bool operator ==(OfferEntity? left, OfferEntity? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(OfferEntity? left, OfferEntity? right)
	{
		return !Equals(left, right);
	}

	#endregion
}