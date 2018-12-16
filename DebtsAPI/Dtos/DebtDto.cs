using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Dtos
{
    public class DebtDto
    {
        public int GiverId { get; set; }
        public int TakerId { get; set; }
        public int Sum { get; set; }
    }
}
