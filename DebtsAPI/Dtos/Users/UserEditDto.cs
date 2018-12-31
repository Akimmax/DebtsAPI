using System.ComponentModel.DataAnnotations;

namespace DebtsAPI.Dtos
{
    public class UserEditDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Email is required and must be properly formatted.")]
        public string Email { get; set; }
        
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
