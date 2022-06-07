using Pg.Rsww.RedTeam.DataStorage.Models;

namespace TourOperator.Models.Entities;

public class TourEntity : Entity, IEquatable<TourEntity>
{
	public string Title { get; set; } = null!;

	public string Arrival { get; set; } = null!;

	public string City { get; set; } = null!;

	public string Country { get; set; } = null!;

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public decimal Price { get; set; }

	public bool Enabled { get; set; }

	#region Equatable

	public bool Equals(TourEntity? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Title == other.Title && Arrival == other.Arrival && City == other.City && Country == other.Country &&
		       StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate) && Price == other.Price &&
		       Enabled == other.Enabled;
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
		return HashCode.Combine(Title, Arrival, City, Country, StartDate, EndDate, Price, Enabled);
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