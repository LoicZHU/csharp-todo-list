using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todo_list.Entities;

public class Avatar
{
	[Key]
	public Guid AvatarId { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string link { get; set; }

	[Required]
	public DateTime CreatedAt { get; set; }

	[ForeignKey(nameof(User) + "Id")]
	public Guid UserId { get; set; }
	public User User { get; set; }
}
