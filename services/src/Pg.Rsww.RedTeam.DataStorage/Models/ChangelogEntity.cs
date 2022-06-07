namespace Pg.Rsww.RedTeam.DataStorage.Models;

public class ChangelogEntity : Entity
{
	public DateTime DateTime { get; set; }
	public string Database { get; set; }
	public string New { get; set; }
	public string Old { get; set; }
}