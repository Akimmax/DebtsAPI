using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtsAPI.Data;
using DebtsAPI.Models;
using DebtsAPI.Dtos;
using AutoMapper;


namespace DebtsAPI.Services
{

    public interface IDebtsService
    {
        IEnumerable<Debt> GetAll();
        Debt CreateDebt(DebtDto debt);
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
            _context.Debts.Add(debt);
            _context.SaveChanges();
            return debt;
        }
    }
}
