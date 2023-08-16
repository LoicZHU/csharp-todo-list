using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todo_list.Entities;

public class Statistic
{
	[Key]
	public Guid StatisticId { get; set; }

	[ForeignKey(nameof(Todo) + "Id")]
	public Guid TodoId { get; set; }
	public Todo Todo { get; set; }

	[ForeignKey(nameof(Category) + "Id")]
	public Guid CategoryId { get; set; }
	public Category Category { get; set; }

	public int CategoryTimesUsed { get; set; }
}
