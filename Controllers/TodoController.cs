using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_list.Helpers;
using todo_list.Services.TodoRepository;

namespace todo_list.Controllers;

[ApiController]
[Route("/api/todos")]
[Authorize(Policy = Policy.AllowAdministrators)]
public class TodoController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly ITodoRepository _todoRepository;

	public TodoController(IMapper mapper, ITodoRepository todoRepository)
	{
		_mapper = mapper;
		_todoRepository = todoRepository;
	}

	// [HttpGet("{id}")]
	// public async Task<IActionResult> GetTodo(Guid id)
	// {
	//
	// }

	[HttpGet]
	public async Task<IActionResult> GetTodos([FromQuery] string page, Guid id)
	{
		var isPageParamInvalid = !int.TryParse(page, out var pageNumber) || pageNumber <= 0;
		if (isPageParamInvalid)
		{
			return BadRequest();
		}

		try
		{
			var todos = await _todoRepository.GetTodos(pageNumber);

			return Ok(todos);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
