using System.ComponentModel.DataAnnotations;

namespace SurveyPortal.Models.Company
{
    public class Company
    {
        public long CompanyID { get; set; }

        [Required(ErrorMessage = "Please enter a company name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a company description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter a Country of origin.")]
        public string? Country { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));

        public ICollection<Survey.Survey.Survey>? Surveys { get; set; } = new List<Survey.Survey.Survey>();
    }
}
