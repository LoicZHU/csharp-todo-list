namespace todo_list.Models;

public class JwtSettings
{
	public string Audience { get; set; }
	public string Issuer { get; set; }
	public int ExpirationMinutes { get; set; }
	public string SecretKey { get; set; }
}
