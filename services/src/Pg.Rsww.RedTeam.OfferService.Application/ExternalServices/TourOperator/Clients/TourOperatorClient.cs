using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Settings;

namespace Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Clients;

public class TourOperatorClient
{
	private readonly HttpClient _client;
	private readonly ILogger<TourOperatorClient> _logger;
	private readonly TourOperatorSettings _settings;

	public TourOperatorClient(
		HttpClient client, 
		IOptions<TourOperatorSettings> settings,
		ILogger<TourOperatorClient> logger
	)
	{
		_client = client;
		_logger = logger;
		_settings = settings.Value;
		client.BaseAddress = new Uri(_settings.Url);
	}

	public async Task<T?> GetAsync<T>(string endpoint)
	{
		try
		{
			var response = await _client.GetAsync(endpoint);
			if (response is not { IsSuccessStatusCode: true })
			{
				return default;
			}

			var contentStream = await response.Content.ReadAsStreamAsync();
			var serializer = new JsonSerializer();

			using (var sr = new StreamReader(contentStream))
			using (var jsonTextReader = new JsonTextReader(sr))
			{
				var obj = serializer.Deserialize<T>(jsonTextReader);
				return obj;
			}
		}
		catch (Exception ex)
		{
			_logger.Log(LogLevel.Error,"Could not collect data from tour operator",ex);
			return default;
		}
	}
}