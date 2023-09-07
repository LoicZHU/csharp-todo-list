using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_list.Helpers;
using todo_list.Services.CategoryRepository;

namespace todo_list.Controllers;

[ApiController]
[Route("/api/categories")]
[Authorize(Policy = Policy.AllowAdministrators)]
public class CategoryController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly ICategoryRepository _categoryRepository;

	public CategoryController(IMapper mapper, ICategoryRepository categoryRepository)
	{
		_mapper = mapper;
		_categoryRepository = categoryRepository;
	}

	[HttpGet]
	public async Task<IActionResult> GetCategories()
	{
		try
		{
			var categories = await _categoryRepository.GetCategories();

			return Ok(categories);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
