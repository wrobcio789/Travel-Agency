namespace Pg.Rsww.RedTeam.EventHandler.Settings;

public class RabbitMQSettings
{
	public string HostName { get; set; } = null!;
	public int Port { get; set; } = 5672;
	public string UserName { get; set; } = null!;
	public string Password { get; set; } = null!;
}