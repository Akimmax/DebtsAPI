using DebtsAPI.Models;
using System;
using System.Collections.Generic;

namespace DebtsAPI.Dtos.Debts
{
    public class DebtDetailPaymentsResponseDto
    {
        public DebtDetailPaymentsResponseDto()
        {
            Payments = new List<PaymentResponseDto>();
        }

        public int Id { get; set; }
        public UserDto Giver { get; set; }
        public UserDto Taker { get; set; }
        public int Sum { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public string Description { get; set; }
        public bool IsRepaid { get; set; }

        public  List<PaymentResponseDto> Payments { get; set; }
    }
}
