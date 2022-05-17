using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.EventHandler;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.OfferService.Application.Events;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Clients;
using Pg.Rsww.RedTeam.OfferService.Application.ExternalServices.TourOperator.Settings;
using Pg.Rsww.RedTeam.OfferService.Application.Repositories;
using Pg.Rsww.RedTeam.OfferService.Application.Workers;

namespace Pg.Rsww.RedTeam.OfferService.Application;

public static class SetupExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddTourOperator(services, configuration);
		AddDatabase(services, configuration);
		AddEventHandler(services, configuration);
		services.AddTransient<Services.OfferService>();
		services.AddTransient<Services.LoaderService>();
		services.AddHostedService<LoaderWorker>();
		return services;
	}

	private static void AddTourOperator(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<TourOperatorSettings>(configuration.GetSection(nameof(TourOperatorSettings)));
		services.AddHttpClient();
		services.AddTransient<TourOperatorClient>();
	}

	private static void AddEventHandler(IServiceCollection services, IConfiguration configuration)
	{
		services.AddEventHandler(configuration);

		services.AddTransient<IQueueCommand, CancelReservationQueueCommand>();
		services.AddTransient<IQueueCommand, CompleteReservationQueueCommand>();

		services.AddTransient<IRpcServerCommand, ReserveRpcServerCommand>();
	}

	private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
		services.AddSingleton<TourRepository>();
		services.AddSingleton<TransportRepository>();
		services.AddSingleton<HotelRepository>();
		services.AddSingleton<OfferRepository>();
	}
}