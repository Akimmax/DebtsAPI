using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DebtsAPI.Data;
using DebtsAPI.Dtos;
using DebtsAPI.Models;
using DebtsAPI.Services.Exceptions;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DebtsAPI.Services
{
    public interface IPaymentService
    {
        Debt CreateDebt(PaymentInboxDto paymentDto);
        Debt PaymentOperations(Debt debt, Payment payment, bool isFullRepayment);
    }

    public class PaymentService : IPaymentService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(DatabaseContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Debt CreateDebt(PaymentInboxDto paymentDto)
        {            
            var debt = _context.Debts.Include(t => t.Payments)
              .FirstOrDefault(p => p.Id == paymentDto.DebtId);
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

            var repaid = PaymentOperations(debt, payment, false);

            _context.SaveChanges();
            return repaid;           
        }

        public Debt PaymentOperations(Debt debt, Payment payment, bool isFullRepayment)
        {
            var currentPaidAmount = debt.Payments.Sum(p => p.Amount);
            var newPaidAmount = currentPaidAmount + payment.Amount;

            if ((newPaidAmount < debt.Sum) && isFullRepayment == false)
            {
                debt.Payments.Add(payment);
            }
            else if ((newPaidAmount == debt.Sum) && isFullRepayment == false)
            {
                debt.Payments.Add(payment);
                debt.IsRepaid = true;
            }
            else if ((newPaidAmount > debt.Sum) || isFullRepayment == true)
            {
                int notRepaidAmount = debt.Sum - currentPaidAmount;
                payment.Amount = notRepaidAmount;
                debt.Payments.Add(payment);
                debt.IsRepaid = true;
            }

            return debt;
        }


    }
}
