namespace Pg.Rsww.RedTeam.EventHandler.Commands;

public interface IRpcServerCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<string>> Command { get; set; }
}