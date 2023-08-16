using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using todo_list.Helpers;

namespace todo_list.Entities;

public enum RoleName
{
	[StringValue("Administrator")]
	Admin,

	[StringValue("User")]
	User,
}

public class Role : IdentityRole<string>
{
	[Key]
	public Guid RoleId { get; set; }

	[MaxLength(100, ErrorMessage = $"{nameof(Name)} must be at most 100 characters.")]
	[Required]
	public RoleName Name { get; set; }
}
