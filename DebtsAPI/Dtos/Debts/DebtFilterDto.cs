namespace DebtsAPI.Dtos.Debts
{
    public class DebtFilterDto
    {
        /// <summary>
        /// Options:
        ///
        ///     ["Giver", "Taker"]
        /// </summary>
        public string RoleInDebt { get; set; }
    }
}
