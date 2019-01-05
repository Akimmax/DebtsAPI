using System.ComponentModel.DataAnnotations;

namespace DebtsAPI.Dtos
{
    public class RealUserMarkerDto
    {
        [EmailAddress(ErrorMessage = "Not email")]
        public string Email { get; set; }
    }
}