using System.Security.Cryptography;
using System.Text;

namespace todo_list.Helpers;

public static class ShaPassword
{
	private static string Salt { get; set; }

	public static string SetPassword(string password)
	{
		using var rng = new RNGCryptoServiceProvider();
		var saltBytes = new byte[16];
		rng.GetBytes(saltBytes);
		Salt = Convert.ToBase64String(saltBytes);

		return ShaPassword.GetHashedPassword(password, Salt);
	}

	public static bool VerifyPassword(string hashedPassword, string password)
	{
		var inputHash = ShaPassword.GetHashedPassword(password, Salt);

		return hashedPassword == inputHash;
	}

	private static string GetHashedPassword(string password, string salt)
	{
		using var sha256 = SHA256.Create();
		var combinedBytes = Encoding.UTF8.GetBytes($"{password}{salt}");
		var hashBytes = sha256.ComputeHash(combinedBytes);

		return Convert.ToBase64String(hashBytes);
	}
}
