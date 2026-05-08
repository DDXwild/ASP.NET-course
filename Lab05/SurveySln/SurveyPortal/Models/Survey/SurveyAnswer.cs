using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyPortal.Models.Survey.Survey
{
    public class SurveyAnswers
    {
        [Key]
        public long SurveyAnswerID { get; set; }

        [ForeignKey("Survey")]
        public long SurveyID { get; set; }

        public Survey Survey { get; set; }

        [Required(ErrorMessage ="Please enter an answer")]
        public string Answer { get; set; } = string.Empty;

        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
    }
}
