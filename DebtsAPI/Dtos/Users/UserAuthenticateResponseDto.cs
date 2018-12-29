using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Dtos
{
    public class UserAuthenticateResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }    
}
