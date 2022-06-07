using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.EventHandler.Services;
using Pg.Rsww.RedTeam.OrderService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OrderService.Application.Workers;

public class OutdatedOrdersWorker : BackgroundService
{
	private readonly OrderRepository _orderRepository;
	private readonly QueueSendService _sendService;
	private readonly ILogger<OutdatedOrdersWorker> _logger;
	private const string QueueName = "cancel-reservation";

	public OutdatedOrdersWorker(
		OrderRepository orderRepository, 
		QueueSendService sendService,
		ILogger<OutdatedOrdersWorker> logger
	)
	{
		_orderRepository = orderRepository;
		_sendService = sendService;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		await Task.Run(async () =>
		{
			while (true)
			{
				var start = DateTime.Now;
				try
				{
					var maxDateTime = DateTime.UtcNow.AddMinutes(-1);
					var reservations = await _orderRepository.CancelOutdatedReservations(maxDateTime);
					if (reservations.Any())
					{
						var jsonBody = JsonConvert.SerializeObject(reservations.Select(x => x.Id));
						_sendService.SendMessage(jsonBody, QueueName);
						jsonBody = JsonConvert.SerializeObject(reservations.Select(x => x.OfferId));
						_sendService.SendMessage(jsonBody, QueueName+"2");
					}
				}
				catch (Exception ex)
				{
					_logger.Log(LogLevel.Error,$"Could not cancel old orders {ex}");
				}

				var timeToWait = DateTime.Now - start;
				if (timeToWait < TimeSpan.FromSeconds(30))
				{
					Thread.Sleep(timeToWait);
				}
			}
		}, cancellationToken);
	}
}