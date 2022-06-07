using Microsoft.AspNetCore.SignalR;

namespace Pg.Rsww.RedTeam.OfferService.Api.Hubs
{
	public class OfferHub : Hub
	{
		public async Task SendMessage(string type, string content)
		{
			await Clients.All.SendAsync("Message", type, content);
		}
	}
}