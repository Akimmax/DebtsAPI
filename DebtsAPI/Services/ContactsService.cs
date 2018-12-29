﻿using System;
using System.Collections.Generic;
using System.Linq;
using DebtsAPI.Models;
using DebtsAPI.Settings;
using DebtsAPI.Data;
using AutoMapper;
using DebtsAPI.Dtos;
using DebtsAPI.Services.Exeptions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using DebtsAPI.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


using DebtsAPI.Services.Exceptions;

namespace DebtsAPI.Services
{
    public interface IContactsService
    {
        void AddRalationship(int id);
        void AddToContacts(int secondUserId);
        void MarkAsSeen(int inviterId);

        IEnumerable<UserContacts> GetAll();
        IEnumerable<UserDto> GetAllForCurrentUser();
        void Delete(int secondUserId);

        void MarkAsSeen(IEnumerable<int> seen);
    }

    public class ContactsService : IContactsService
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactsService(DatabaseContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        public void AddRalationship(int secondUserId)
        {
            var firstUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var firstUser = _context.Users.Find(firstUserId);
            var secondUser = _context.Users.Find(secondUserId);

            if (firstUser == null || secondUser == null)
            {
                throw new NotFoundException();
            }

            _context.UserContacts.Add(new UserContacts { UserId = firstUser.Id, ContactId = secondUser.Id });

            _context.SaveChanges();
        }

        public void AddToContacts(int secondUserId)
        {
            var firstUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var firstUser = _context.Users.Find(firstUserId);
            var secondUser = _context.Users.Find(secondUserId);

            if (firstUser == null || secondUser == null)
            {
                throw new NotFoundException();
            }

            _context.UserContacts.Add(new UserContacts { UserId = firstUser.Id, ContactId = secondUser.Id });
            _context.UserContacts.Add(new UserContacts { UserId = secondUser.Id, ContactId = firstUser.Id });

            _context.SaveChanges();
        }

        public void MarkAsSeen(int inviterId)
        {
            var receiverId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var contact = _context.UserContacts.FirstOrDefault(c => c.UserId == inviterId && c.ContactId == receiverId);

            if (contact == null )
            {
                throw new NotFoundException();
            }

            contact.IsRead = true;
            _context.UserContacts.Update(contact);           

            _context.SaveChanges();
        }

        public IEnumerable<UserContacts> GetAll()
        {
            var contacts = _context.UserContacts;

            return contacts;
        }

        public void MarkAsSeen(IEnumerable<int> seenId)
        {
            var receiverId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var invations = _context.UserContacts.Where(c => c.ContactId == receiverId);

            var seenInvations = invations.Where(u => seenId.Any(a => a == u.UserId));//select request which wos seen

            if (seenInvations == null || !seenInvations.Any() )
            {
                throw new NotFoundException();
            }

            foreach (var invation in seenInvations)
            {
                invation.IsRead = true;
                _context.UserContacts.Update(invation);
            }

            _context.SaveChanges();
        }

        public IEnumerable<UserDto> GetAllForCurrentUser()
        {
            var currentUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = _context.Users
                .Include(t => t.SentInvitations).ThenInclude(t => t.Contact)
                .Include(t => t.ReceivedInvitations).ThenInclude(t => t.User)
                .FirstOrDefault(u => u.IsActive == true && u.Id == currentUserId);

            var senders = user.SentInvitations.Select(p => p.Contact);//users whom user considers as contacts
            var receivers = user.ReceivedInvitations.Select(c => c.User);//users who consider user as contact 

            //users who consider user as contact and user considers them as contacts
            var validContact = senders.Intersect(receivers); 

            return _mapper.Map<IEnumerable<UserDto>>(validContact);
        }

        public void Delete(int secondUserId)
        {
            var currentUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var contact = _context.UserContacts.FirstOrDefault(c => c.UserId == currentUserId && c.ContactId == secondUserId);
         
            if (contact == null)
            {
                throw new NotFoundException();
            }
            if (contact.UserId != currentUserId)
            {
                throw new ForbiddenException();
            }

            _context.UserContacts.Remove(contact);
            _context.SaveChanges();

        }




    }
}