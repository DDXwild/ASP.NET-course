using Microsoft.EntityFrameworkCore;

namespace SurveyPortal.Models.Survey.Survey
{
    public class EFSurveyRepository : ISurveyRepository
    {
        private readonly SurveyDbContext _context;
        public EFSurveyRepository(SurveyDbContext context)
        {
            _context = context;
        }
        public IQueryable<Survey> Surveys => _context.Surveys.Include(s => s.SurveyAnswers);
        public IQueryable<Company.Company> Companies => _context.Companies.Include(c => c.Surveys);

        public void AddAnswer(SurveyAnswers answer)
        {
            _context.SurveyAnswers.Add(answer);
            _context.SaveChanges();
        }
        public void SaveSurvey(Survey survey)
        {
            _context.Surveys.Update(survey);
            _context.SaveChanges();
        }
        public void AddSurvey(Survey survey)
        {
            _context.Surveys.Add(survey);
            _context.SaveChanges();
        }
        public void DeleteSurvey(Survey survey)
        {
            _context.Surveys.Remove(survey);
            _context.SaveChanges();
        }

        public void AddCompany(Company.Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
        }

        public void SaveCompany(Company.Company company)
        {
            _context.Companies.Update(company);
            _context.SaveChanges();
        }

        public void DeleteCompany(Company.Company company)
        {
            _context.Companies.Remove(company);
            _context.SaveChanges();
        }
    }
}
