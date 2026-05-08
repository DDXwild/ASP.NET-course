using System.ComponentModel.DataAnnotations;
namespace SurveyPortal.Models.Identity.DTO
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The password must be at least {2} and at most {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string? NewPasswordConfirmation { get; set; }
    }
}