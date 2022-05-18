using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pg.Rsww.RedTeam.OrderService.Api.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var user = context.HttpContext.Items["CustomerId"];
		if (user == null)
		{
			// not logged in
			context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
		}
	}
}