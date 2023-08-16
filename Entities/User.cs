using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace todo_list.Entities;

public class User
{
	[Key]
	public Guid UserId { get; set; }

	[EmailAddress]
	[Required]
	public string Email { get; set; }

	[StringLength(
		maximumLength: 255,
		MinimumLength = 8,
		ErrorMessage = $"{nameof(Password)} must be b/w 8 & 255 characters."
	)]
	[Required]
	public string Password { get; private set; }

	[Required]
	public DateTime CreatedAt { get; set; }

	public DateTime? UpdatedAt { get; set; }

	[ForeignKey(nameof(Role) + "Id")]
	public Role Role { get; set; }
	public Guid RoleId { get; set; }

	public void SetPassword(string password)
	{
		using var sha256 = SHA256.Create();

		var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
		Password = Convert.ToBase64String(hashBytes);
	}

	public bool VerifyPassword(string password)
	{
		using var sha256 = SHA256.Create();

		var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
		var inputHash = Convert.ToBase64String(hashBytes);

		return Password == inputHash;
	}
}
