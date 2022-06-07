namespace Pg.Rsww.RedTeam.OfferService.Application.Services;

public class StatisticsAggregate
{
	public List<TransportStatistics> Transports { get; set; }
	public List<HotelStatistics> Hotels { get; set; }
	public List<TourPlaceStatistics> Tours { get; set; }
}
public class TourPlaceStatistics
{
	public string Departure { get; set; }
	public int Visits { get; set; }
}

public class HotelStatistics
{
	public string HotelName { get; set; }
	public int Visits { get; set; }
}

public class TransportStatistics
{
	public string Code { get; set; }
	public int Visits { get; set; }
}