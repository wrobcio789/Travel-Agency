using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.EventHandler.Settings;
using Pg.Rsww.RedTeam.EventHandler.Workers;

namespace Pg.Rsww.RedTeam.EventHandler.Services;

public class QueueReceiveService : BackgroundService
{
	private readonly IEnumerable<IQueueCommand> _commands;
	private readonly ILogger<QueueReceiveService> _logger;
	private readonly RabbitMQSettings _rabbitMqSettings;

	public QueueReceiveService(
		IEnumerable<IQueueCommand> commands,
		IOptions<RabbitMQSettings> rabbitMqSettings,
		ILogger<QueueReceiveService> logger
	)
	{
		_commands = commands;
		_logger = logger;
		_rabbitMqSettings = rabbitMqSettings.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (!_commands.Any())
		{
			return;
		}
		var consumers = new List<QueueWorker>();
		var tasks = new List<Task>();
		foreach (var action in _commands)
		{
			var consumer = new QueueWorker(_rabbitMqSettings, action.QueueName, action.Command, _logger);
			consumers.Add(consumer);
			tasks.Add(consumer.StartAsync(stoppingToken));
		}
		
		Task.WaitAll(tasks.ToArray());
	}
}