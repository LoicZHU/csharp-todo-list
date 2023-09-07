using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_list.Helpers;
using todo_list.Services.StatisticRepository;

namespace todo_list.Controllers;

[ApiController]
[Route("/api/statistics")]
[Authorize(Policy = Policy.AllowAdministrators)]
public class StatisticController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IStatisticRepository _statisticRepository;

	public StatisticController(IMapper mapper, IStatisticRepository statisticRepository)
	{
		_mapper = mapper;
		_statisticRepository = statisticRepository;
	}

	[HttpGet]
	public async Task<IActionResult> GetStatistics()
	{
		try
		{
			var statistics = await _statisticRepository.GetStatistics();

			return Ok(statistics);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
