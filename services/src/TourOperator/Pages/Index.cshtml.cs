using Microsoft.AspNetCore.Mvc.RazorPages;
using Pg.Rsww.RedTeam.DataStorage.Models;
using Pg.Rsww.RedTeam.DataStorage.Repositories;

namespace TourOperator.Pages;

public class IndexModel : PageModel
{
	private readonly ChangelogRepository _repository;
	public List<ChangelogEntity> Changes { get; set; }

	public IndexModel(ChangelogRepository repository)
	{
		_repository = repository;
	}

	public async Task OnGet()
	{
		Changes = await _repository.GetNewestAsync(10);
	}
}