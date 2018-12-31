using System;

namespace DebtsAPI.Dtos.Debts
{
    public class DebtDetailResponseDto
    {
        public int Id { get; set; }
        public UserDto Giver { get; set; }
        public UserDto Taker { get; set; }
        public int Sum { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
