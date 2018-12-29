using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DebtsAPI.Services.Exeptions;
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
            var contacts = _contactService.GetAll().ToList();

            return Ok(contacts);
        }

        [HttpPost]
        [Route("create/{secondUserId}")]
        public IActionResult MakeContactsForEachOther(int secondUserId)
        {
            try
            {
                _contactService.AddToContacts(secondUserId);
                return Ok();
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("invite/{receiverId}")]
        public IActionResult SendInvitationToContacts(int receiverId)
        {
            try
            {
                _contactService.AddRalationship(receiverId);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("accept/{inviterId}")]
        public IActionResult AcceptInvitations(int inviterId)
        {
            try
            {
                _contactService.AddRalationship(inviterId);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("seen/{senderId}")]
        public IActionResult MarkAsSeen(int senderId)
        {
            try
            {
                _contactService.MarkAsSeen(senderId);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("seenmany")]
        public IActionResult MarkAsSeenMany([FromBody]List<int> sendersId)
        {           
            try
            {
                _contactService.MarkAsSeen(sendersId);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("forcurrentuser")]
        public IActionResult GetAllForCurrentUser()
        {
            var users = _contactService.GetAllForCurrentUser().ToList();

            return Ok(users);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _contactService.Delete(id);
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