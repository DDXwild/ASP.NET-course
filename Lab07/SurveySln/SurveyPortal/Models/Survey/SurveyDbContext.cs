using Microsoft.EntityFrameworkCore;
using SurveyPortal.Models.Survey.Survey;
using SurveyPortal.Models.Company;

namespace SurveyPortal.Models.Survey.Survey
{
    public class SurveyDbContext : DbContext
    {
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options) { }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Company.Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Survey>()
                .HasOne(s => s.Company)
                .WithMany(c => c.Surveys)
                .HasForeignKey(s => s.CompanyID)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<SurveyAnswers> SurveyAnswers { get; set; }
    }
}
