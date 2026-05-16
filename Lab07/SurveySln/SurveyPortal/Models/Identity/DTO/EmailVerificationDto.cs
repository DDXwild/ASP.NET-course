using System.ComponentModel.DataAnnotations;

namespace SurveyPortal.Models.Identity.DTO
{
    public class EmailVerificationDto
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is not found.")]
        public string? Email { get; set; }
    }
}
