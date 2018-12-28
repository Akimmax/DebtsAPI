﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Models
{
    public class UserContacts
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ContactId { get; set; }
        public virtual User Contact { get; set; }

        public bool IsRead { get; set; }
    }
}
