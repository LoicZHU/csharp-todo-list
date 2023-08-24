using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todo_list.Entities;

public class User
{
	[Key]
	public Guid UserId { get; set; }

	[EmailAddress]
	[Required]
	[Index(nameof(Email), IsUnique = true)]
	public string Email { get; set; }

	[StringLength(
		maximumLength: 255,
		MinimumLength = 8,
		ErrorMessage = $"{nameof(Password)} must be b/w 8 & 255 characters."
	)]
	[Required]
	public string Password { get; set; }

	[Required]
	public DateTime CreatedAt { get; set; }

	public DateTime? UpdatedAt { get; set; }

	[ForeignKey(nameof(Role) + "Id")]
	[Required]
	public Guid RoleId { get; set; }
	public Role Role { get; set; }
}
