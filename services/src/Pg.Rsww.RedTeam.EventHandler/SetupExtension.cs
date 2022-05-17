using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pg.Rsww.RedTeam.EventHandler.Services;
using Pg.Rsww.RedTeam.EventHandler.Settings;

namespace Pg.Rsww.RedTeam.EventHandler;

public static class SetupExtension
{
	public static IServiceCollection AddEventHandler(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));

		services.AddTransient<RpcClientService>();
		services.AddHostedService<RpcServerService>();

		services.AddHostedService<QueueReceiveService>();
		services.AddTransient<QueueSendService>();

		return services;
	}
}