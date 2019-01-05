using System;
using System.ComponentModel.DataAnnotations;

namespace DebtsAPI.Dtos.Debts
{
    public class DebtEditDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sum should be positive")]
        public int Sum { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public string Description { get; set; }
    }
}
