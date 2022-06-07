namespace Pg.Rsww.RedTeam.DataStorage.Extensions;

public static class StringExtensions
{
	public static string SurroundWithQuotes(this string text)
	{
		return $"\"{text}\"";
	}
}