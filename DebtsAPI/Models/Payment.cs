namespace DebtsAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int DebtId { get; set; }
        public virtual Debt Debt { get; set; }

        public string Description { get; set; }
        public int Amount { get; set; }

    }
}
