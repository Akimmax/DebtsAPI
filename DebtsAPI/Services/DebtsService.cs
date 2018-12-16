using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtsAPI.Data;
using DebtsAPI.Models;
using DebtsAPI.Dtos;


namespace DebtsAPI.Services
{

    public interface IDebtsService
    {
        IEnumerable<Debt> GetAll();
        Debt CreateDebt(DebtDto debt);
    }

    public class DebtsService: IDebtsService
    {
        private readonly DatabaseContext _context;
        public DebtsService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Debt> GetAll()
        {
            return _context.Debts;
        }

        public Debt CreateDebt(DebtDto debtDto)
        {
            Debt debt = new Debt { GiverId = debtDto.GiverId, TakerId = debtDto.TakerId, Sum = debtDto.Sum,Date = DateTimeOffset.Now };

            _context.Debts.Add(debt);
            _context.SaveChanges();
            return debt;
        }
    }
}
