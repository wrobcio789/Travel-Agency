using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.OfferService.Api.Hubs;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Events;

public class CompleteReservationQueueCommand : IQueueCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<bool>> Command { get; set; }

	private readonly OfferRepository _offerRepository;
	private readonly IHubContext<OfferHub> _hubContext;

	public CompleteReservationQueueCommand(OfferRepository offerRepository, IHubContext<OfferHub> hubContext)
	{
		_offerRepository = offerRepository;
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
			await _hubContext.Clients.All.SendAsync("Message", "OfferBought", JsonConvert.SerializeObject(offer.TourId));
		}
		return success;
	}
	
}