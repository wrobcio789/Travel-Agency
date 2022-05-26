using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pg.Rsww.RedTeam.EventHandler.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pg.Rsww.RedTeam.EventHandler.Workers;

public class QueueWorker : BackgroundService
{
	private readonly string _queueName;
	private readonly Func<string, Task<bool>> _eventHandler;
	private readonly ILogger _logger;
	private readonly RabbitMQSettings _rabbitMqSettings;

	public QueueWorker(
		RabbitMQSettings rabbitMqSettings,
		string queueName,
		Func<string, Task<bool>> eventHandler,
		ILogger logger
	)
	{
		_queueName = queueName;
		_eventHandler = eventHandler;
		_logger = logger;
		_rabbitMqSettings = rabbitMqSettings;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Run(() =>
		{
			while (true)
			{
				try
				{
					var factory = new ConnectionFactory()
					{
						HostName = _rabbitMqSettings.HostName, Port = _rabbitMqSettings.Port,
						UserName = _rabbitMqSettings.UserName, Password = _rabbitMqSettings.Password
					};
					using (var connection = factory.CreateConnection())
					using (var channel = connection.CreateModel())
					{
						channel.QueueDeclare(queue: _queueName,
							durable: true,
							exclusive: false,
							autoDelete: false,
							arguments: null);

						var consumer = new EventingBasicConsumer(channel);
						consumer.Received += async (model, ea) =>
						{
							var body = ea.Body.ToArray();
							var message = Encoding.UTF8.GetString(body);
							await _eventHandler(message);
						};
						channel.BasicConsume(queue: _queueName,
							autoAck: true,
							consumer: consumer);

						while (true)
						{
							Thread.Sleep(100);
						}
					}
				}
				catch (Exception ex)
				{
					_logger.Log(LogLevel.Error, $"Queue error {ex}");
				}

				Thread.Sleep(5000);
			}
		}, stoppingToken);
	}
}