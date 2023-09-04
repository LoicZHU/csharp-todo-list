using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Primitives;

namespace todo_list.Utils;

public static class TokenUtils
{
	public static JwtSecurityToken? ReadToken(StringValues headerAuth)
	{
		var token = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];

		return new JwtSecurityTokenHandler().ReadJwtToken(token);
	}

	public static string? GetUserId(JwtSecurityToken? token)
	{
		return token?.Claims
			.FirstOrDefault(c => string.Equals(c.Type, "nameId", StringComparison.OrdinalIgnoreCase))
			?.Value;
	}

	public static string? GetUserRole(JwtSecurityToken? token)
	{
		return token?.Claims.FirstOrDefault(c => string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase))?.Value;
	}
}
