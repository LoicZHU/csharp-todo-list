using System.ComponentModel.DataAnnotations;

namespace todo_list.Entities;

public class Category
{
	[Key]
	public Guid CategoryId { get; set; }

	[StringLength(
		maximumLength: 255,
		MinimumLength = 1,
		ErrorMessage = $"{nameof(Name)} must be b/w 1 & 255 characters."
	)]
	[Required]
	public string Name { get; set; }

	public ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
}
