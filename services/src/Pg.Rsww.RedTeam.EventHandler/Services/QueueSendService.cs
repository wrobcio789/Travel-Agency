using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Logging;
using Pg.Rsww.RedTeam.EventHandler.Settings;

namespace Pg.Rsww.RedTeam.EventHandler.Services;

public class QueueSendService
{
	private readonly ILogger<QueueSendService> _logger;
	private readonly RabbitMQSettings _rabbitMqSettings;

	public QueueSendService(
		IOptions<RabbitMQSettings> rabbitMqSettings,
		ILogger<QueueSendService> logger
	)
	{
		_logger = logger;
		_rabbitMqSettings = rabbitMqSettings.Value;
	}

	public bool SendMessage(string message, string queue)
	{
		try
		{
			var factory = new ConnectionFactory() { HostName = _rabbitMqSettings.HostName , Port= _rabbitMqSettings.Port, UserName=_rabbitMqSettings.UserName,Password = _rabbitMqSettings.Password};
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: queue,
					durable: true,
					exclusive: false,
					autoDelete: false,
					arguments: null);

				var body = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish(exchange: "",
					routingKey: queue,
					basicProperties: null,
					body: body);
			}
		}
		catch (Exception ex)
		{
			_logger.Log(LogLevel.Error,$"Could not send message {ex}");
			return false;
		}

		return true;
	}
}