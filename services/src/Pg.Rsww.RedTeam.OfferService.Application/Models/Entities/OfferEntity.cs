using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.DataStorage.Models;

namespace Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;

public class OfferEntity : Entity
{

	public string TourId { get; set; } = null!;

	public Reservation Reservation { get; set; } = null!;

	public People People { get; set; } = null!;

	public int GetTicketsCount() =>
		People.Adults + People.Teenagers + People.Children + People.Toddlers;
}