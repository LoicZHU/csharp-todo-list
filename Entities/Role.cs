using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace todo_list.Entities;

// [JsonConverter(typeof(StringEnumConverter))]
public enum RoleName
{
	Administrator,
	User,
}

[Index(nameof(Name), IsUnique = true)]
public class Role
{
	[Key]
	public Guid RoleId { get; set; }

	[Required]
	public RoleName Name { get; set; }
}
