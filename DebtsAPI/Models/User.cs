using System.Collections.Generic;

namespace DebtsAPI.Models
{
    public class User
    {
        public User()
        {
            SentInvitations = new List<UserContacts>();
            ReceivedInvitations = new List<UserContacts>();

            GivenDebts = new List<Debt>();
            TakenDebts = new List<Debt>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsVirtual { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<UserContacts> SentInvitations { get; set; }
        public virtual ICollection<UserContacts> ReceivedInvitations { get; set; }

        public virtual ICollection<Debt> GivenDebts { get; set; }
        public virtual ICollection<Debt> TakenDebts { get; set; }

    }    
}
