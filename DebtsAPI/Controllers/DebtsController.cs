using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using DebtsAPI.Models;
using DebtsAPI.Services;
using DebtsAPI.Dtos.Debts;
using DebtsAPI.Services.Exceptions;
using DebtsAPI.Services.Helpers;

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
        public IActionResult GetAll([FromQuery] DebtFilterDto debtFilterDto)
        {
            return Ok(_debtsService.GetAll(debtFilterDto));
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public IActionResult EditDebt(int id, [FromBody] DebtEditDto debtDto)
        {
            debtDto.Id = id;

            try
            {                
                _debtsService.Update(debtDto);
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
