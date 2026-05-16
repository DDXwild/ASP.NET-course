using System.ComponentModel.DataAnnotations;

namespace SurveyPortal.Models.Identity
{
    public class VerifyEmail
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
