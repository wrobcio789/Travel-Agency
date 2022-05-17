using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using Pg.Rsww.RedTeam.EventHandler.Settings;

namespace Pg.Rsww.RedTeam.EventHandler.Services;

public class RpcClientService
{
	private readonly IConnection connection;
	private readonly IModel channel;
	private readonly string replyQueueName;
	private readonly EventingBasicConsumer consumer;
	private readonly BlockingCollection<string> respQueue = new();
	private readonly IBasicProperties props;
	private readonly RabbitMQSettings _rabbitMqSettings;

	public RpcClientService(IOptions<RabbitMQSettings> rabbitMQSettings)
	{
		_rabbitMqSettings = rabbitMQSettings.Value;
		var factory = new ConnectionFactory() { HostName = _rabbitMqSettings.HostName };

		connection = factory.CreateConnection();
		channel = connection.CreateModel();
		replyQueueName = channel.QueueDeclare().QueueName;
		consumer = new EventingBasicConsumer(channel);

		props = channel.CreateBasicProperties();
		var correlationId = Guid.NewGuid().ToString();
		props.CorrelationId = correlationId;
		props.ReplyTo = replyQueueName;

		consumer.Received += (model, ea) =>
		{
			var body = ea.Body.ToArray();
			var response = Encoding.UTF8.GetString(body);
			if (ea.BasicProperties.CorrelationId == correlationId)
			{
				respQueue.Add(response);
			}
		};

		channel.BasicConsume(
			consumer: consumer,
			queue: replyQueueName,
			autoAck: true);
	}

	public string Call(string message, string queueName)
	{
		var messageBytes = Encoding.UTF8.GetBytes(message);
		channel.BasicPublish(
			exchange: "",
			routingKey: queueName,
			basicProperties: props,
			body: messageBytes);

		return respQueue.Take();
	}

	public void Close()
	{
		connection.Close();
	}
}