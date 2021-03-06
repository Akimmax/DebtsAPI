﻿using System.ComponentModel.DataAnnotations;

namespace DebtsAPI.Dtos
{
    public class UserAuthenticateDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email is required and must be properly formatted.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
