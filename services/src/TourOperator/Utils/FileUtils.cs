using Newtonsoft.Json;

namespace TourOperator.Utils;

public static class FileUtils
{
	public static T GetData<T>(string filename)
	{
		var filepath = Path.Join("Data", filename);
		var json = File.ReadAllText(filepath);
		var obj = JsonConvert.DeserializeObject<T>(json);
		return obj;
	}
}