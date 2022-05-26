using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.EventHandler.Services;
using Pg.Rsww.RedTeam.OrderService.Application.Repositories;

namespace Pg.Rsww.RedTeam.OrderService.Application.Events;

public class PaymentMadeQueueCommand : IQueueCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<bool>> Command { get; set; }
	private readonly OrderRepository _orderRepository;
	private readonly QueueSendService _queueSendService;

	public PaymentMadeQueueCommand(
		OrderRepository orderRepository,
		QueueSendService queueSendService
	)
	{
		_orderRepository = orderRepository;
		_queueSendService = queueSendService;
		QueueName = "payment-made";
		Command = HandleMessage;
	}

	private async Task<bool> HandleMessage(string orderId)
	{
		orderId = orderId.Replace("\"", string.Empty);
		await _orderRepository.UpdateStatus(orderId, ReservationStatus.Completed);
		var order = await _orderRepository.GetById(orderId);
		if (order != null)
		{
			_queueSendService.SendMessage(order.OfferId, "complete-reservation");
		}

		return true;
	}
}