using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using todo_list.Entities;
using todo_list.Models;

namespace todo_list.Services.Auth;

public class AuthService
{
	private readonly JwtSettings _jwtSettings;

	public AuthService(IOptions<JwtSettings> jwtSettings)
	{
		_jwtSettings = jwtSettings.Value;
	}

	public string GenerateJwt(User user)
	{
		// csharpier-ignore
    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
      new Claim(ClaimTypes.Email, user.Email),
      new Claim(ClaimTypes.Role, user.Role?.Name ?? "No role specified"),
    };

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Audience = _jwtSettings.Audience,
			Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
			Issuer = _jwtSettings.Issuer,
			SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
			Subject = new ClaimsIdentity(claims),
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}
