using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace todo_list.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Role
{
	[Key]
	public Guid RoleId { get; set; }

	[Required]
	public string Name { get; set; }
}
