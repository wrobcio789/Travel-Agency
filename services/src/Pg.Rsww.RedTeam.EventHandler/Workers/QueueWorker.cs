using System.Text;
using Microsoft.Extensions.Hosting;
using Pg.Rsww.RedTeam.EventHandler.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pg.Rsww.RedTeam.EventHandler.Workers;

public class QueueWorker : BackgroundService
{
	private readonly string _queueName;
	private readonly Func<string, Task<bool>> _eventHandler;
	private readonly RabbitMQSettings _rabbitMqSettings;

	public QueueWorker(
		RabbitMQSettings rabbitMqSettings,
		string queueName,
		Func<string, Task<bool>> eventHandler
	)
	{
		_queueName = queueName;
		_eventHandler = eventHandler;
		_rabbitMqSettings = rabbitMqSettings;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Run(() =>
		{
			var factory = new ConnectionFactory() { HostName = _rabbitMqSettings.HostName };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: _queueName,
					durable: false,
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
		}, stoppingToken);
	}
}