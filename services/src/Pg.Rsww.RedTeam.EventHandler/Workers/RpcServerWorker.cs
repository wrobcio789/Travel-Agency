using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pg.Rsww.RedTeam.EventHandler.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pg.Rsww.RedTeam.EventHandler.Workers;

public class RpcServerWorker : BackgroundService
{
	private readonly string _queueName;
	private readonly Func<string, Task<string>> _messageHandler;
	private readonly ILogger<RpcServerWorker> _logger;
	private readonly RabbitMQSettings _rabbitMqSettings;

	public RpcServerWorker(
		RabbitMQSettings rabbitMqSettings,
		string queueName,
		Func<string, Task<string>> messageHandler,
		ILogger<RpcServerWorker> logger
	)
	{
		_queueName = queueName;
		_messageHandler = messageHandler;
		_rabbitMqSettings = rabbitMqSettings;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		await Task.Run(async () =>
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
						channel.QueueDeclare(queue: _queueName, durable: true,
							exclusive: false, autoDelete: false, arguments: null);
						channel.BasicQos(0, 1, false);
						var consumer = new EventingBasicConsumer(channel);
						channel.BasicConsume(queue: _queueName,
							autoAck: false, consumer: consumer);

						consumer.Received += async (model, ea) =>
						{
							string response = null;

							var body = ea.Body.ToArray();
							var props = ea.BasicProperties;
							var replyProps = channel.CreateBasicProperties();
							replyProps.CorrelationId = props.CorrelationId;

							try
							{
								var message = Encoding.UTF8.GetString(body);

								response = await _messageHandler(message);
							}
							catch (Exception e)
							{
								Console.WriteLine(" [.] " + e.Message);
								response = "";
							}
							finally
							{
								var responseBytes = Encoding.UTF8.GetBytes(response);
								channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
									basicProperties: replyProps, body: responseBytes);
								channel.BasicAck(deliveryTag: ea.DeliveryTag,
									multiple: false);
							}
						};
						while (true)
						{
							Thread.Sleep(100);
						}
					}
				}
				catch (Exception ex)
				{
					_logger.Log(LogLevel.Error, $"{nameof(RpcServerWorker)} error {ex}");
				}

				Thread.Sleep(5000);
			}
		}, cancellationToken);
	}
}