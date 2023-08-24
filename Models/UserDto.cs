using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using todo_list.Entities;

namespace todo_list.Models;

public class UserDto
{
	[EmailAddress]
	public string Email { get; set; }

	[StringLength(
		maximumLength: 255,
		MinimumLength = 8,
		ErrorMessage = $"{nameof(Password)} must be b/w 8 & 255 characters."
	)]
	public string Password { get; private set; }

	public DateTime? UpdatedAt { get; set; }

	[ForeignKey(nameof(Role) + "Id")]
	public Guid RoleId { get; set; }
	public Role Role { get; set; }

	// avatars
}
