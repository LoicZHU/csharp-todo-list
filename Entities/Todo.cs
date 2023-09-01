using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace todo_list.Entities;

[JsonConverter(typeof(StringEnumConverter))]
public enum TodoStatus
{
	Closed,
	Completed,
	InProgress,
	Open
}

public class Todo
{
	[Key]
	public Guid TodoId { get; set; }

	[MaxLength(100, ErrorMessage = $"{nameof(Title)} must be at most 100 characters.")]
	[Required]
	public string Title { get; set; }

	[MaxLength(255, ErrorMessage = $"{nameof(Description)} must be at most 255 characters.")]
	public string? Description { get; set; }

	public TodoStatus Status { get; set; } = TodoStatus.Open;

	[Required]
	public DateTime CreatedAt { get; set; }

	[Required]
	public DateTime? UpdatedAt { get; set; }

	[ForeignKey(nameof(User) + "Id")]
	public Guid UserId { get; set; }
	public User User { get; set; }

	public ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
}
