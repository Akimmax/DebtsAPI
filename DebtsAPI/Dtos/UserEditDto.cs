using DebtsAPI.Dtos.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Dtos
{
    public class UserEditDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ValidEmail(ErrorMessage = "Email is required and must be properly formatted.")]//Allows null
        public string Email { get; set; }
        
        [StringLength(50, MinimumLength = 6)]//Allows null
        public string Password { get; set; }
    }
}
