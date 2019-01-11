using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using DebtsAPI.Data;
using DebtsAPI.Models;
using DebtsAPI.Dtos.Debts;
using DebtsAPI.Services.Exceptions;

namespace DebtsAPI.Services
{

    public interface IDebtsService
    {
        IEnumerable<DebtDetailResponseDto> GetAll(DebtFilterDto debtFilterDto);
        void CreateDebt(DebtInboxDto debt);
        void Delete(int id);
        void Update(DebtEditDto debtDto);
        DebtDetailPaymentsResponseDto GetById(int id);
        Debt Repay(PaymentInboxDto paymentDto);
    }

    public class DebtsService : IDebtsService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;        
        private readonly DebtDtoMapper _debtDtoMapper;
        private readonly IPaymentService _paymentService;

        public DebtsService(DatabaseContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, DebtDtoMapper debtDtoMapper, IPaymentService paymentService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _debtDtoMapper = debtDtoMapper;
            _paymentService = paymentService;

        }

        public IEnumerable<DebtDetailResponseDto> GetAll(DebtFilterDto debtFilterDto)
        {
           
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var unfilteredDebts = _context.Debts
                .Where(debt => debt.GiverId == userId || debt.TakerId == userId);

            switch (debtFilterDto.RoleInDebt)
            {
                case Constants.Debts.UserRole.GIVER:
                    unfilteredDebts = unfilteredDebts
                        .Where(debt => debt.GiverId == userId);
                    break;
                case Constants.Debts.UserRole.TAKER:
                    unfilteredDebts = unfilteredDebts
                        .Where(debt => debt.TakerId == userId);
                    break;
                default:
                    unfilteredDebts = unfilteredDebts
                        .Where(debt => debt.GiverId == userId || debt.TakerId == userId);
                    break;
            }

            var userDebts = unfilteredDebts
                .Include(debt => debt.Giver)
                .Include(debt => debt.Taker)
                .Select(debt => _debtDtoMapper.MapToDebtDetailResponseDto(debt));

            return userDebts;
        }

        public void CreateDebt(DebtInboxDto debtDto)
        {
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool isAccessAllow = userId == debtDto.GiverId || userId == debtDto.TakerId;

            if (!isAccessAllow)
            {
                throw new ForbiddenException();
            }


            Debt debt = _mapper.Map<Debt>(debtDto);

            debt.IsRepaid = false;
            debt.Date = DateTimeOffset.Now;

            _context.Debts.Add(debt);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var debt = _context.Debts.Find(id);
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            if (debt == null)
            {
                throw new NotFoundException();

            }

            bool isAccessAllow = userId == debt.GiverId || userId == debt.TakerId;

            if (!isAccessAllow)
            {
                throw new ForbiddenException();
            }

            _context.Debts.Remove(debt);
            _context.SaveChanges();
        }

        public void Update(DebtEditDto debtDto)
        {
           var debt = _context.Debts.Include(t => t.Payments).FirstOrDefault(p => p.Id == debtDto.Id);
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (debt == null)
            {
                throw new NotFoundException();
            }

            bool isAccessAllow = userId == debt.GiverId || userId == debt.TakerId;

            if (!isAccessAllow)
            {
                throw new ForbiddenException();
            }

            if ((debt.Sum != debtDto.Sum) && (debtDto.Sum < debt.Payments.Sum(p => p.Amount)))
            {
                debt.IsRepaid = true;
            }

            debt.Sum = debtDto.Sum;
            debt.Deadline = debtDto.Deadline;
            debt.Description = debtDto.Description;

            _context.SaveChanges();
        }

        public DebtDetailPaymentsResponseDto GetById(int id)
        {
            var debt = _context.Debts
                .Include(d => d.Giver)
                .Include(d => d.Taker)
                .Include(d => d.Payments)
                .FirstOrDefault(p => p.Id == id);

            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (debt == null)
            {
                throw new NotFoundException();
            }

            bool isAccessAllow = userId == debt.GiverId || userId == debt.TakerId;

            if (!isAccessAllow)
            {
                throw new ForbiddenException();
            }          

            var debtDto = _debtDtoMapper.MapToDebtDetailPaymentsResponseDto(debt);

            return debtDto;
        }

        public Debt Repay(PaymentInboxDto paymentDto)
        {
            var debt = _context.Debts.Include(t => t.Payments).FirstOrDefault(p => p.Id == paymentDto.DebtId);
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (debt == null)
            {
                throw new NotFoundException();
            }

            bool isAccessAllow = userId == debt.GiverId || userId == debt.TakerId;

            if (!isAccessAllow)
            {
                throw new ForbiddenException();
            }

            Payment payment = _mapper.Map<Payment>(paymentDto);

            var repaid = _paymentService.PaymentOperations(debt, payment, true);

            _context.SaveChanges();
            return repaid;
        }
    }
}
