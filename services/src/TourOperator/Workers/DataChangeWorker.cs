using TourOperator.ExternalServices.OfferService.Clients;
using TourOperator.Models.Entities;
using TourOperator.Services;

namespace TourOperator.Workers;

public class DataChangeWorker : BackgroundService
{
	private readonly DataChangeService _service;
	private readonly OfferServiceClient _client;
	private readonly ILogger<LoaderWorker> _logger;

	public DataChangeWorker(
		DataChangeService service,
		OfferServiceClient client,
		ILogger<LoaderWorker> logger
	)
	{
		_service = service;
		_client = client;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		await Task.Run(async () =>
		{
			Thread.Sleep(TimeSpan.FromMinutes(1));

			var waitTime = TimeSpan.FromSeconds(30);
			while (true)
			{
				var changes = new List<TourEntity>();
				try
				{
					changes.AddRange(await _service.ChangeEnabled(3));
					changes.AddRange(await _service.ChangeRandomPrice(3));
					changes.AddRange(await _service.ChangeRandomTitle(3));
				}
				catch (Exception ex)
				{
					_logger.Log(LogLevel.Error, $"{nameof(DataChangeWorker)} error {ex}");
					throw;
				}

				if (changes.Any())
				{
					await _client.PostAsync("TourDelta", changes);
				}

				Thread.Sleep(waitTime);
			}
		}, cancellationToken);
	}
}