using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyPortal.Models.Survey.Survey
{
    public class Survey
    {
        [Key]
        public long? SurveyID { get; set; }

        [Required(ErrorMessage = "Please enter a survey title")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter a disription")]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
        [Required(ErrorMessage = "Please enter a category")]
        public string? Category { get; set; }
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Please chose a company of survey")]
        public long? CompanyID { get; set; }
        public Company.Company? Company { get; set; }

        public ICollection<SurveyAnswers> SurveyAnswers { get; set; } = new List<SurveyAnswers>();
    }
}