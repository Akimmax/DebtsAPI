using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DebtsAPI.Services;

namespace DebtsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet]
        public IActionResult GetTotalStatistic()
        {
            return Ok(_statisticService.GetTotalStatistic());
        }

    }
}
