using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DebtsAPI.Services;
using DebtsAPI.Dtos;
using DebtsAPI.Models;
using System.Security.Claims;
using DebtsAPI.Services.Exceptions;

namespace DebtsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private IContactsService _contactService;


        public ContactsController(IContactsService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _contactService.GetAllForCurrentUser();
            return Ok(users);
        }

        [HttpPost]
        [Route("create/{secondUserId}")]
        public IActionResult Create(int secondUserId)
        {
            try
            {
                _contactService.AddToContacts(secondUserId);
                return Ok();
            }
            catch (ContactNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }           
        }

        [HttpPost]
        [Route("invite/{receiverId}")]
        public IActionResult Invite(int receiverId)
        {
            try
            {
                _contactService.AddRelationship(receiverId);
                return Ok();
            }
            catch (ContactNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("accept/{inviterId}")]
        public IActionResult AcceptInvitations(int inviterId)
        {
            try
            {
                _contactService.AddRelationship(inviterId);
                return Ok();
            }
            catch (ContactNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("seen")]
        public IActionResult MarkAsSeenMany([FromBody]List<UserContactsDto> sendersId)
        {           
            try
            {
                _contactService.MarkAsSeen(sendersId);
                return Ok();
            }
            catch (ContactNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _contactService.Delete(id);
                return Ok();
            }
            catch (ContactNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
        }

    }
}