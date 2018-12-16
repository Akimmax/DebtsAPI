using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public int GiverId { get; set; }
        public int TakerId { get; set; }
        public int Sum { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
