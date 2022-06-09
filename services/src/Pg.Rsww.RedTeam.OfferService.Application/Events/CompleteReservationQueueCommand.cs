using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.OfferService.Api.Hubs;
using Pg.Rsww.RedTeam.OfferService.Application.Models.Entities;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Events;

public class CompleteReservationQueueCommand : IQueueCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<bool>> Command { get; set; }

	private readonly OfferRepository _offerRepository;
	private readonly StatisticsRepository _statisticsRepository;
	private readonly HotelRepository _hotelRepository;
	private readonly TransportRepository _transportRepository;
	private readonly IHubContext<OfferHub> _hubContext;

	public CompleteReservationQueueCommand(OfferRepository offerRepository, StatisticsRepository statisticsRepository,
		HotelRepository hotelRepository, TransportRepository transportRepository, IHubContext<OfferHub> hubContext)
	{
		_offerRepository = offerRepository;
		_statisticsRepository = statisticsRepository;
		_hotelRepository = hotelRepository;
		_transportRepository = transportRepository;
		_hubContext = hubContext;
		QueueName = "complete-reservation";
		Command = HandleMessage;
	}

	private async Task<bool> HandleMessage(string offerId)
	{
		var updatesCount = await _offerRepository.UpdateStatus(offerId, ReservationStatus.Completed);
		var success = updatesCount == 1;
		if (success)
		{
			var offer = await _offerRepository.GetAsync(offerId);
			var transportStart = await _transportRepository.GetAsync(offer.Reservation.StartTransport);
			var transportEnd = await _transportRepository.GetAsync(offer.Reservation.EndTransport);
			var hotel = await _hotelRepository.GetAsync(offer.Reservation.Accommodation.HotelId);

			var increment = offer.GetTicketsCount();

			if (hotel != null)
			{
				await _statisticsRepository.Add(StatisticsDomains.Hotel, hotel.Name, increment);
			}

			if (transportStart != null)
			{
				await _statisticsRepository.Add(StatisticsDomains.Tour, transportStart.Departure, increment);
				await _statisticsRepository.Add(StatisticsDomains.Transport, transportStart.Type.ToString(), increment);
			}

			if (transportEnd != null)
			{
				await _statisticsRepository.Add(StatisticsDomains.Transport, transportEnd.Type.ToString(), increment);
			}

			await _hubContext.Clients.All.SendAsync("Message", "OfferBought", offer.TourId);
		}

		return success;
	}
}