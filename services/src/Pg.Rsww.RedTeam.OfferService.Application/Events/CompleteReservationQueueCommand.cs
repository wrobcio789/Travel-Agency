using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Events;

public class CompleteReservationQueueCommand : IQueueCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<bool>> Command { get; set; }

	private readonly OfferRepository _offerRepository;

	public CompleteReservationQueueCommand(OfferRepository offerRepository)
	{
		_offerRepository = offerRepository;
		QueueName = "complete-reservation";
		Command = HandleMessage;
	}

	private async Task<bool> HandleMessage(string offerId)
	{
		var updatesCount = await _offerRepository.UpdateStatus(offerId, ReservationStatus.Cancelled);
		return updatesCount == 1;
	}
	
}