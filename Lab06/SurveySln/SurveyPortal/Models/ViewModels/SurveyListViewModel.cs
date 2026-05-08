
namespace SurveyPortal.Models.ViewModels
{
    public class SurveyListViewModel
    {
        public IEnumerable<Survey.Survey.Survey> Surveys { get; set; } = new List<Survey.Survey.Survey>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
        public string? CurrentCategory { get; set; }

        public long? CurrentCompany { get; set; }
    }
}
