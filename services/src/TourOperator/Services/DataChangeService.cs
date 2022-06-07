using System.Text.RegularExpressions;
using TourOperator.Repositories;

namespace TourOperator.Services;

public class DataChangeService
{
	private readonly TourRepository _tourRepository;

	public DataChangeService(TourRepository tourRepository)
	{
		_tourRepository = tourRepository;
	}

	public async Task ChangeRandomPrice(int count)
	{
		var random = new Random();
		var records = await _tourRepository.GetRandomAsync(count);
		foreach (var record in records)
		{
			record.Price = random.Next(1000, 3000);
		}

		await _tourRepository.UpsertAsync(records);
	}

	public async Task ChangeRandomTitle(int count)
	{
		var records = await _tourRepository.GetRandomAsync(count);
		foreach (var record in records)
		{
			var addBaseVersion = true;
			var match = Regex.Match(record.Title, "new v(\\d+)");
			if (match.Success)
			{
				var successParse = int.TryParse(match.Groups[1].Value, out var version);
				if (successParse)
				{
					record.Title = record.Title.Replace(match.Groups[1].Value, (version + 1).ToString());
					addBaseVersion = false;
				}
			}

			if (addBaseVersion)
			{
				record.Title = $"{record.Title} new v1";
			}
		}

		await _tourRepository.UpsertAsync(records);
	}

	public async Task ChangeEnabled(int count)
	{
		var records = await _tourRepository.GetRandomAsync(count);
		foreach (var record in records)
		{
			record.Enabled = !record.Enabled;
		}

		await _tourRepository.UpsertAsync(records);
	}
}