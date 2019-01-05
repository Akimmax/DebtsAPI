using System;
using System.ComponentModel.DataAnnotations;

namespace DebtsAPI.Dtos.Debts
{
    public class DebtInboxDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Giver ID required")]
        public int GiverId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Taker ID required")]
        public int TakerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sum should be positive")]
        public int Sum { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public string Description { get; set; }
    }
}
