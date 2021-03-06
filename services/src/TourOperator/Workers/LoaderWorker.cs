using TourOperator.Services;

namespace TourOperator.Workers;

public class LoaderWorker : BackgroundService
{
	private readonly LoaderService _service;
	private readonly ILogger<LoaderWorker> _logger;

	public LoaderWorker(
		LoaderService service,
		ILogger<LoaderWorker> logger
	)
	{
		_service = service;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		await Task.Run(async () =>
		{
			var tryCount = 3;
			var initialWaitTime = TimeSpan.FromSeconds(5);
			var incrementWaitTime = TimeSpan.FromSeconds(10);
			while (tryCount > 0)
			{
				var success = await _service.FullLoadAsync(false);
				if (success)
				{
					_logger.Log(LogLevel.Information, "Initial data loaded successfully");
					break;
				}

				_logger.Log(LogLevel.Information, "Could not load initial data");
				initialWaitTime += incrementWaitTime;
				tryCount--;
				Thread.Sleep(initialWaitTime);
			}
		}, cancellationToken);
	}
}