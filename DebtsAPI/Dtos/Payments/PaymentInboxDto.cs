using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Models
{
    public class PaymentInboxDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debt ID required")]
        public int DebtId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be positive")]
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
