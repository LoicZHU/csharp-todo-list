using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using todo_list.Helpers;

namespace todo_list.Entities;

// [JsonConverter(typeof(StringEnumConverter))]
public enum RoleName
{
	// [StringValue("Administrator")]
	[EnumMember(Value = "Administrator")]
	// [Display(Name = "Administrator")]
	Administrator,

	// [StringValue("User")]
	[EnumMember(Value = "User")]
	// [Display(Name = "User")]
	User,
}

public class Role
{
	[Key]
	public Guid RoleId { get; set; }

	// [MaxLength(100, ErrorMessage = $"{nameof(Name)} must be at most 100 characters.")]
	[Required]
	// [JsonConverter(typeof(JsonStringEnumConverter))]
	// [JsonConverter(typeof(StringEnumConverter))]
	// [JsonProperty("Name")]
	public RoleName Name { get; set; }
}
