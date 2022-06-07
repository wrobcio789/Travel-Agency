using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using TourOperator.Repositories;
using TourOperator.Services;
using TourOperator.Workers;

namespace TourOperator.Extensions;

public static class SetupExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddDatabase(services, configuration);
		services.AddTransient<LoaderService>();
		services.AddHostedService<LoaderWorker>();
		return services;
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