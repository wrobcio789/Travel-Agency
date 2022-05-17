using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.EventHandler.Settings;
using Pg.Rsww.RedTeam.EventHandler.Workers;

namespace Pg.Rsww.RedTeam.EventHandler.Services;

public class RpcServerService : BackgroundService
{
	private readonly IEnumerable<IRpcServerCommand> _commands;
	private readonly RabbitMQSettings _rabbitMqSettings;

	public RpcServerService(
		IEnumerable<IRpcServerCommand> commands,
		IOptions<RabbitMQSettings> rabbitMqSettings
	)
	{
		_commands = commands;
		_rabbitMqSettings = rabbitMqSettings.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (!_commands.Any())
		{
			return;
		}
		var consumers = new List<RpcServerWorker>();
		var tasks = new List<Task>();
		foreach (var action in _commands)
		{
			var consumer = new RpcServerWorker(_rabbitMqSettings, action.QueueName, action.Command);
			consumers.Add(consumer);
			tasks.Add(consumer.StartAsync(stoppingToken));
		}

		Task.WaitAll(tasks.ToArray());
	}
}