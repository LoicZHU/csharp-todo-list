using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using todo_list.Entities;
using todo_list.Helpers;

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
      new Claim(ClaimTypes.Role, user.RoleId.ToString()),
    };

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
			SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}
