﻿using DebtsAPI.Dtos.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DebtsAPI.Dtos
{
    public class UserAuthenticateDto
    {
        [Required]
        [ValidEmail(ErrorMessage = "Email is required and must be properly formatted.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
