using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using DebtsAPI.Models;
using DebtsAPI.Services;
using DebtsAPI.Dtos;
using DebtsAPI.Services.Exceptions;

namespace DebtsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _debtsService.Delete(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
        }
    }
}
