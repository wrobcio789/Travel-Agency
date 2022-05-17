﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.EventHandler.Services;
using Pg.Rsww.RedTeam.OrderService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OrderService.Application.Workers;

public class OutdatedOrdersWorker : BackgroundService
{
	private readonly OrderRepository _orderDbService;
	private readonly QueueSendService _sendService;
	private readonly ILogger<OutdatedOrdersWorker> _logger;
	private const string QueueName = "cancel-reservation";

	public OutdatedOrdersWorker(
		OrderRepository orderDbService, 
		QueueSendService sendService,
		ILogger<OutdatedOrdersWorker> logger
	)
	{
		_orderDbService = orderDbService;
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
					var reservations = await _orderDbService.CancelOutdatedReservations(maxDateTime);
					if (reservations.Any())
					{
						var jsonBody = JsonConvert.SerializeObject(reservations.Select(x => x.OfferId));
						_sendService.SendMessage(jsonBody, QueueName);
					}
				}
				catch (Exception ex)
				{
					_logger.Log(LogLevel.Error,"Could not cancel old orders",ex);
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