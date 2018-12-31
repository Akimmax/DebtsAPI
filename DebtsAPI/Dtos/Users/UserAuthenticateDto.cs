using System.ComponentModel.DataAnnotations;

namespace DebtsAPI.Dtos
{
    public class UserAuthenticateDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is required and must be properly formatted.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
