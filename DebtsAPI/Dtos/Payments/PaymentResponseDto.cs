using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Models
{
    public class PaymentResponseDto
    {
        public int DebtId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
