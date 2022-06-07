using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using TourOperator.ExternalServices.OfferService.Clients;
using TourOperator.ExternalServices.OfferService.Settings;
using TourOperator.Repositories;
using TourOperator.Services;
using TourOperator.Workers;

namespace TourOperator.Extensions;

public static class SetupExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddDatabase(services, configuration);
		AddOfferService(services, configuration);

		services.AddTransient<LoaderService>();
		services.AddHostedService<LoaderWorker>();

		services.AddTransient<DataChangeService>();
		services.AddHostedService<DataChangeWorker>();

		return services;
	}

	private static void AddOfferService(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<OfferServiceSettings>(configuration.GetSection(nameof(OfferServiceSettings)));
		services.AddHttpClient();
		services.AddTransient<OfferServiceClient>();
	}
	private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
		services.AddSingleton<TourRepository>();
		services.AddSingleton<TransportRepository>();
		services.AddSingleton<HotelRepository>();
		services.AddSingleton<OfferRepository>();
		services.AddSingleton<ChangelogRepository>();
	}
}