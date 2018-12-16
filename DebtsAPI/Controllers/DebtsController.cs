using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DebtsAPI.Models;
using DebtsAPI.Services;
using DebtsAPI.Dtos;

namespace DebtsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtsService _debtsService;

        public DebtsController(IDebtsService debtsService)
        {
            _debtsService = debtsService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_debtsService.GetAll());
        }

        [HttpPost]
        public ActionResult CreateDebt([FromBody] DebtDto debtDto)
        {
            try
            {
                Debt debt = _debtsService.CreateDebt(debtDto);
                return Ok(debt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
