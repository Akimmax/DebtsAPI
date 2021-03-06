﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DebtsAPI.Services.Exceptions;
using DebtsAPI.Services;
using DebtsAPI.Dtos;
using DebtsAPI.Models;

namespace DebtsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]    
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        
        public UsersController(IUserService userService)        
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserAuthenticateDto userDto)
        {
            var authenticatedUser = _userService.Authenticate(userDto.Email, userDto.Password);

            if (authenticatedUser == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }                

            return Ok(authenticatedUser);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserAuthenticateDto userDto)
        { 
            try
            {
                _userService.Create(userDto);
                return Ok();
            }
            catch (UserException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public IActionResult CreateVirtual([FromBody] UserDto userDto){
            try
            {
                UserDto user = _userService.CreateVirtual(userDto);
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest();
            }
     
        }

        [HttpPut]
        [Route("connecttoreal/{virtualUserId}")]
        public IActionResult ConnectToReal(int virtualUserId, [FromBody] RealUserMarkerDto realUserMarkerDto)
        {
            try
            {
                _userService.ConnectToReal(virtualUserId, realUserMarkerDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();           
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);         
            return Ok(user);
        }

        [HttpPut]
        public IActionResult Update([FromBody]UserEditDto userDto)
        {
            try
            {
                _userService.Update(userDto);
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}