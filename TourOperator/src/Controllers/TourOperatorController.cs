using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TourOperator.Models;

namespace TourOperator.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TourOperatorController : ControllerBase
	{
		private readonly ILogger<TourOperatorController> _logger;

		public TourOperatorController(ILogger<TourOperatorController> logger)
		{
			_logger = logger;
		}

		[HttpGet("Hotels")]
		public List<Hotel> GetAccommodations()
		{
			return GetData<List<Hotel>>("hotels.json");
		}


		[HttpGet("Transports")]
		public List<Transport> GetTransports()
		{
			return GetData<List<Transport>>("transports.json");
		}

		[HttpGet("Tours")]
		public List<Tour> GetOffers()
		{
			return GetData<List<Tour>>("tours.json");
		}

		private static T GetData<T>(string filename)
		{
			var filepath = Path.Join("Data", filename);
			var json = System.IO.File.ReadAllText(filepath);
			var obj = JsonConvert.DeserializeObject<T>(json);
			return obj;
		}
	}
}