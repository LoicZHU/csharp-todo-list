using Microsoft.AspNetCore.Mvc;

namespace todo_list.Helpers;

public static class ControllerBaseExtensions
{
	public static IActionResult HandleError(
		this ControllerBase controllerBase,
		string errorMessage = "An internal server error occurred.",
		int statusCode = StatusCodes.Status500InternalServerError
	)
	{
		Console.WriteLine(errorMessage);
		return new ObjectResult("An internal server error occurred.") { StatusCode = statusCode };
	}
}
