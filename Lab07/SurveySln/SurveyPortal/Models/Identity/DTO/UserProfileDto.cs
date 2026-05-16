using System.ComponentModel.DataAnnotations;

namespace SurveyPortal.Models.Identity.DTO
    {
        public class UserProfileDto
        {
            public string Id { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "First name is required")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Phone Number")]
            [Phone(ErrorMessage = "Invalid phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public DateTime? DateOfBirth { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "State/Province")]
            public string State { get; set; }

            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Display(Name = "Country")]
            public string Country { get; set; }

            [Display(Name = "Profile Picture")]
            public string ProfilePictureUrl { get; set; }
        }
    }