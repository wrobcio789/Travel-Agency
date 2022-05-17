using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.EventHandler;
using Pg.Rsww.RedTeam.EventHandler.Commands;
using Pg.Rsww.RedTeam.OrderService.Application.Events;
using Pg.Rsww.RedTeam.OrderService.Application.Repositories;
using Pg.Rsww.RedTeam.OrderService.Application.Workers;

namespace Pg.Rsww.RedTeam.OrderService.Application;

public static class SetupExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddDatabase(services, configuration);
		AddEventHandler(services, configuration);
		services.AddTransient<Services.OrderService>();

		return services;
	}

	private static void AddEventHandler(IServiceCollection services, IConfiguration configuration)
	{
		services.AddEventHandler(configuration);
		services.AddHostedService<OutdatedOrdersWorker>();
		services.AddTransient<IQueueCommand, PaymentMadeQueueCommand>();
	}

	private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
		services.AddSingleton<OrderRepository>();
	}
}