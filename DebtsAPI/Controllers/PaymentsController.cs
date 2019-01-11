using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using DebtsAPI.Services;
using DebtsAPI.Dtos.Debts;
using DebtsAPI.Services.Exceptions;
using DebtsAPI.Models;

namespace DebtsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentsService;

        public PaymentsController(IPaymentService paymentsService)
        {
            _paymentsService = paymentsService;
        }
      
        [HttpPost]
        [ProducesResponseType(403)]
        public IActionResult CreateDebt([FromBody] PaymentInboxDto payment)
        {
            try
            {
                _paymentsService.CreateDebt(payment);
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
