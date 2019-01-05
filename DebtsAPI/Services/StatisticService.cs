using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DebtsAPI.Data;
using DebtsAPI.Dtos;

namespace DebtsAPI.Services
{
    public interface IStatisticService
    {
        StatisticDto GetTotalStatistic();
    }

    public class StatisticService : IStatisticService
    {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StatisticService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public StatisticDto GetTotalStatistic()
        {
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            StatisticDto statistic = new StatisticDto()
            {
                Gave = _context.Debts.Where(debt => debt.GiverId == userId).Sum(n => n.Sum),
                Took = _context.Debts.Where(debt => debt.TakerId == userId).Sum(n => n.Sum)
            };

            return statistic;
        }
    }
}
