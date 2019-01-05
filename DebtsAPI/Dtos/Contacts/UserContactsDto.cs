using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DebtsAPI.Models
{
    public class UserContactsDto
    {
        public int UserId { get; set; }
    }
}
