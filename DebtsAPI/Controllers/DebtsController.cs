using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using DebtsAPI.Services;
using DebtsAPI.Dtos.Debts;
using DebtsAPI.Services.Exceptions;

namespace DebtsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtsService _debtsService;

        public DebtsController(IDebtsService debtsService)
        {
            _debtsService = debtsService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DebtFilterDto debtFilterDto)
        {
            return Ok(_debtsService.GetAll(debtFilterDto));
        }

        [HttpPost]
        [ProducesResponseType(403)]
        public IActionResult CreateDebt([FromBody] DebtInboxDto debtDto)
        {
            try
            {
                _debtsService.CreateDebt(debtDto);
                return Ok();
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(403)]
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
