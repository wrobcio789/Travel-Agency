using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Pg.Rsww.RedTeam.OrderService.Api.Settings;

namespace Pg.Rsww.RedTeam.OrderService.Api.Middleware;

public class JwtHelper
{
	private readonly JwtSettings _settings;
	private readonly ILogger<JwtHelper> _logger;

	public JwtHelper(IOptions<JwtSettings> settings, ILogger<JwtHelper> logger)
	{
		_settings = settings.Value;
		_logger = logger;
	}

	public bool VerifySignature(string jwt)
	{
		string[] parts = jwt.Split(".".ToCharArray());
		var header = parts[0];
		var payload = parts[1];
		var signature = parts[2]; //Base64UrlEncoded signature from the token

		byte[] bytesToSign = getBytes(string.Join(".", header, payload));

		byte[] secret = getBytes(_settings.Secret);

		var alg = new HMACSHA256(secret);
		var hash = alg.ComputeHash(bytesToSign);

		var computedSignature = Base64UrlEncode(hash);

		return computedSignature.Equals(signature);
	}

	private static byte[] getBytes(string value)
	{
		return Encoding.UTF8.GetBytes(value);
	}

	// from JWT spec
	private static string Base64UrlEncode(byte[] input)
	{
		var output = Convert.ToBase64String(input);
		output = output.Split('=')[0]; // Remove any trailing '='s
		output = output.Replace('+', '-'); // 62nd char of encoding
		output = output.Replace('/', '_'); // 63rd char of encoding
		return output;
	}

	public string GetJwtTokenClaim(string token, string claimName)
	{
		try
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
			var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
			return claimValue;
		}
		catch (Exception ex)
		{
			_logger.Log(LogLevel.Error, $"Token is missing claim {claimName} {ex}");
			return null;
		}
	}
}