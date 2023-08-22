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
		return new ObjectResult(errorMessage) { StatusCode = statusCode };
	}
}
