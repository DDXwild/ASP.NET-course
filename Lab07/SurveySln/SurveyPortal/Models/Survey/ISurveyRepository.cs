namespace SurveyPortal.Models.Survey.Survey
{
    public interface ISurveyRepository
    {
        IQueryable<Survey> Surveys { get; }
        IQueryable<Company.Company> Companies { get; }
        void AddAnswer(SurveyAnswers answer);
        void SaveSurvey(Survey existingSurvey);
        void DeleteSurvey (Survey survey);
        void AddSurvey(Survey survey);

        void AddCompany(Company.Company company);
        void SaveCompany(Company.Company company);
        void DeleteCompany(Company.Company company);
    }
}
