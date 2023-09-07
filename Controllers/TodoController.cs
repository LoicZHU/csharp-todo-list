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
	public async Task<IActionResult> GetTodos(Guid id)
	{
		try
		{
			var todos = await _todoRepository.GetTodos();

			return Ok(todos);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
