namespace Pg.Rsww.RedTeam.EventHandler.Commands;

public interface IQueueCommand
{
	public string QueueName { get; set; }
	public Func<string, Task<bool>> Command { get; set; }
}