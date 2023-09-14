using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todo_list.Entities;

public class Statistic
{
	// [ForeignKey(nameof(Todo) + "Id")]
	public Guid TodoId { get; init; }
	public Todo Todo { get; }

	// [ForeignKey(nameof(Category) + "Id")]
	public Guid CategoryId { get; init; }
	public Category Category { get; }
}
