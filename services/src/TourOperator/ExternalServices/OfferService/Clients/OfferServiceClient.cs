using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TourOperator.ExternalServices.OfferService.Settings;

namespace TourOperator.ExternalServices.OfferService.Clients;

public class OfferServiceClient
{
	private readonly HttpClient _client;
	private readonly ILogger<OfferServiceClient> _logger;
	private readonly OfferServiceSettings _settings;

	public OfferServiceClient(
		HttpClient client, 
		IOptions<OfferServiceSettings> settings,
		ILogger<OfferServiceClient> logger
	)
	{
		_client = client;
		_logger = logger;
		_settings = settings.Value;
		client.BaseAddress = new Uri(_settings.Url);
	}

	public async Task<bool> PostAsync<T>(string endpoint, T obj)
	{
		try
		{
			var json = JsonConvert.SerializeObject(obj);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(endpoint,httpContent);
			return response is { IsSuccessStatusCode: true };
		}
		catch (Exception ex)
		{
			_logger.Log(LogLevel.Error,$"{nameof(OfferServiceClient)} {ex}");
			return false;
		}
	}
}