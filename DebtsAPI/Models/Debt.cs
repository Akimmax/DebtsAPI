using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Models
{
    public class Debt
    {
        public Debt()
        {
            Payments = new List<Payment>();
        }

        public int Id { get; set; }

        public int? GiverId { get; set; }
        public virtual User Giver { get; set; }

        public int? TakerId { get; set; }
        public virtual User Taker { get; set; }

        public int Sum { get; set; }
        public bool IsRepaid { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
      
    }
}
