using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pg.Rsww.RedTeam.OfferService.Application.Services;

namespace Pg.Rsww.RedTeam.OfferService.Application.Workers
{
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
			var tryCount = 3;
			var initialWaitTime = TimeSpan.FromSeconds(5);
			var incrementWaitTime = TimeSpan.FromSeconds(10);
			while (tryCount > 0)
			{
				var success = await _service.FullLoadAsync(false);
				if (success)
				{
					_logger.Log(LogLevel.Information,"Initial data loaded successfully");
					break;
				}
				_logger.Log(LogLevel.Information, "Could not load initial data");
				Thread.Sleep(initialWaitTime);
				initialWaitTime += incrementWaitTime;
				tryCount--;
			}
		}
	}
}