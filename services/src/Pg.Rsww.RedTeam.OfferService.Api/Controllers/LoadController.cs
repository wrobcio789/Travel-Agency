using Microsoft.AspNetCore.Mvc;
using Pg.Rsww.RedTeam.OfferService.Application.Services;

namespace Pg.Rsww.RedTeam.OfferService.Api.Controllers;

[Controller]
[Route("api/[controller]")]
public class LoadController : Controller
{
	private readonly LoaderService _loaderService;

	public LoadController(LoaderService loaderService)
	{
		_loaderService = loaderService;
	}

	[HttpGet]
	public async Task<bool> LoadAll()
	{
		var result = await _loaderService.FullLoadAsync();

		return result;
	}
}