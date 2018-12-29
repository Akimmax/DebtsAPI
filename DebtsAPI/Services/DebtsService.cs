using System;
using System.Collections.Generic;
using AutoMapper;

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
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        public DebtsService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            if (debt != null)
            {
                _context.Debts.Remove(debt);
                _context.SaveChanges();
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}
