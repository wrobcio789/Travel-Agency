using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.Common.Models.Offer;
using Pg.Rsww.RedTeam.EventHandler.Commands;

namespace Pg.Rsww.RedTeam.OfferService.Application.Events;

public class ReserveRpcServerCommand : IRpcServerCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<string>> Command { get; set; }
	private readonly Services.OfferService _offerService;
	private ILogger<ReserveRpcServerCommand> _logger;

	public ReserveRpcServerCommand(Services.OfferService offerService, ILogger<ReserveRpcServerCommand> logger)
	{
		_offerService = offerService;
		QueueName = "reserve-order";
		Command = HandleRequest;
		_logger = logger;
	}

	private async Task<string> HandleRequest(string message)
	{
		try
		{
			var offer = JsonConvert.DeserializeObject<OfferRequest>(message);
			if (offer == null)
			{
				return null;
			}

			var responseObj = await _offerService.MakeOfferAsync(offer);

			var response = JsonConvert.SerializeObject(responseObj);
			return response;
		}
		catch (Exception ex)
		{
			_logger.LogError("Could not process command {name}", nameof(ReserveRpcServerCommand),ex);
			return null;
		}
	}
}