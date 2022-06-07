using TourOperator.Services;

namespace TourOperator.Workers
{
	public class DataChangeWorker : BackgroundService
	{
		private readonly DataChangeService _service;
		private readonly ILogger<LoaderWorker> _logger;

		public DataChangeWorker(
			DataChangeService service,
			ILogger<LoaderWorker> logger
			)
		{
			_service = service;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			var waitTime = TimeSpan.FromSeconds(30);
			while (true)
			{
				try
				{
					await _service.ChangeEnabled(3);
					await _service.ChangeRandomPrice(3);
					await _service.ChangeRandomTitle(3);
				}
				catch (Exception ex)
				{
					_logger.Log(LogLevel.Error, $"{nameof(DataChangeWorker)} error {ex}");
					throw;
				}

				Thread.Sleep(waitTime);
			}
		}
	}
}