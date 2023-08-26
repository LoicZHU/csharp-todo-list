using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using todo_list.Entities;

namespace todo_list.Models;

public class UserDto
{
	[StringLength(
		maximumLength: 255,
		MinimumLength = 1,
		ErrorMessage = $"{nameof(Password)} must be b/w 1 & 255 characters."
	)]
	[Required]
	public string FirstName { get; set; }

	[EmailAddress]
	public string Email { get; set; }

	[StringLength(
		maximumLength: 255,
		MinimumLength = 8,
		ErrorMessage = $"{nameof(Password)} must be b/w 8 & 255 characters."
	)]
	public string Password { get; set; }

	[ForeignKey(nameof(Role) + "Id")]
	[Required]
	public Guid? RoleId { get; set; }

	// avatars
}
