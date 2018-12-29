using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

using DebtsAPI.Data;
using DebtsAPI.Models;
using DebtsAPI.Dtos;
using DebtsAPI.Services.Exceptions;



namespace DebtsAPI.Services
{

    public interface IDebtsService
    {
        IEnumerable<Debt> GetAll();
        Debt CreateDebt(DebtDto debt);
        void Delete(int id);
    }

    public class DebtsService : IDebtsService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DebtsService(DatabaseContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Debt> GetAll()
        {
            return _context.Debts;
        }

        public Debt CreateDebt(DebtDto debtDto)
        {
            Debt debt = _mapper.Map<Debt>(debtDto);
            debt.IsActive = true;
            debt.Date = DateTimeOffset.Now;

            _context.Debts.Add(debt);
            _context.SaveChanges();

            return debt;
        }
        public void Delete(int id)
        {
            var debt = _context.Debts.Find(id);
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            if (debt == null)
            {
                throw new NotFoundException();

            }
            if (userId != debt.GiverId && userId != debt.TakerId)
            {
                throw new ForbiddenException();
            }

            _context.Debts.Remove(debt);
            _context.SaveChanges();
        }
    }
}
