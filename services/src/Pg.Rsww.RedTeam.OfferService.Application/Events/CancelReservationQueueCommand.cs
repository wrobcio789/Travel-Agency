using Newtonsoft.Json;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OfferService.Application.Events;

public class CancelReservationQueueCommand : IQueueCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<bool>> Command { get; set; }

	private readonly OfferRepository _offerRepository;

	public CancelReservationQueueCommand(OfferRepository offerRepository)
	{
		_offerRepository = offerRepository;
		QueueName = "cancel-reservation";
		Command = HandleMessage;
	}

	private async Task<bool> HandleMessage(string message)
	{
		var offerIds = JsonConvert.DeserializeObject<List<string>>(message);
		var updatesCount = await _offerRepository.UpdateStatus(offerIds, ReservationStatus.Cancelled);
		return updatesCount == offerIds.Count;
	}
}