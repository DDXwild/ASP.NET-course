namespace SurveyPortal.Models.ViewModels
{
    public class SurveyAnswerMessage
    {
        public long SurveyAnswerID { get; set; }
        public long SurveyID { get; set; }
        public string Answer { get; set; } = string.Empty;
        public DateTime AnsweredAt { get; set; }
    }
}
