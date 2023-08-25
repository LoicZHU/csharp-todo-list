using System.ComponentModel.DataAnnotations;

namespace todo_list.Models;

public class LogInDto
{
	[EmailAddress]
	public string Email { get; set; }

	public string Password { get; set; }
}
