namespace Pg.Rsww.RedTeam.OrderService.Api.Middleware;

public class JwtMiddleware
{
	private readonly RequestDelegate _next;
	private readonly JwtHelper _jwtHelper;

	public JwtMiddleware(RequestDelegate next,JwtHelper jwtHelper)
	{
		_next = next;
		_jwtHelper = jwtHelper;
	}

	public async Task Invoke(HttpContext context)
	{
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		if (token != null)
		{
			AttachCustomerToContext(context, token);
		}

		await _next(context);
	}


	private void AttachCustomerToContext(HttpContext context, string token)
	{
		var valid = _jwtHelper.VerifySignature(token);
		if (!valid)
		{
			return;
		}

		context.Items["CustomerId"] = _jwtHelper.GetJwtTokenClaim(token, "sub");
	}
}