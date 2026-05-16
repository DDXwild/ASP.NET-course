using Microsoft.EntityFrameworkCore;
using SurveyPortal.Models.Survey.Survey;
using SurveyPortal.Models.Company;
using static SurveyPortal.Models.Survey.Survey.Survey;

namespace SurveyPortal.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            SurveyDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<SurveyDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Companies.Any())
            {
                var companies = new List<Company.Company>
                {
                    new Company.Company
                    {
                        Name = "Tech Solutions Inc.",
                        Description = "Leading tech solutions provider",
                        Country = "USA",
                    },
                    new Company.Company
                    {
                        Name = "HealthCare Systems Ltd.",
                        Description = "Innovating healthcare technologies",
                        Country = "UK",
                    },
                    new Company.Company
                    {
                        Name = "EduWorld Co.",
                        Description = "A global leader in education solutions",
                        Country = "Canada",
                    },
                    new Company.Company
                    {
                        Name = "GreenTech Innovations",
                        Description = "Sustainability through green technologies",
                        Country = "Germany",
                    },
                    new Company.Company
                    {
                        Name = "FastTrack Logistics",
                        Description = "Speed and reliability in logistics",
                        Country = "Australia",
                    },
                    new Company.Company
                    {
                        Name = "Foodies Delight",
                        Description = "The ultimate food lover's company",
                        Country = "Italy",
                    }
                };

                context.Companies.AddRange(companies);
                context.SaveChanges();
            }

            if (!context.Surveys.Any())
            {
                Random rand = new Random();
                var companyList = context.Companies.ToList();

                var surveys = new List<Survey.Survey.Survey>
                {
                    new Survey.Survey.Survey
                    {
                        Title = "Customer Satisfaction Survey",
                        Description = "Help us improve our services by sharing your feedback",
                        Category = "Customer Service",
                        CreatedAt = DateTime.UtcNow.AddDays(-30).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[0].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "The service was excellent and exceeded my expectations." },
                            new SurveyAnswers { Answer = "Wait time was too long during peak hours." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Product Feedback Survey",
                        Description = "Tell us what you think about our new product line",
                        Category = "Products",
                        CreatedAt = DateTime.UtcNow.AddDays(-20).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[1].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "The new product design is sleek and user-friendly." },
                            new SurveyAnswers { Answer = "I was hoping for more customization options." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Website Usability Survey",
                        Description = "Rate your experience with our new website design",
                        Category = "Technology",
                        CreatedAt = DateTime.UtcNow.AddDays(-15).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[2].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "The website loads quickly and looks modern." },
                            new SurveyAnswers { Answer = "I had trouble finding the contact page." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Market Research Survey",
                        Description = "Help us understand your preferences and needs",
                        Category = "Research",
                        CreatedAt = DateTime.UtcNow.AddDays(-10).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[3].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "I prefer eco-friendly products even if they cost more." },
                            new SurveyAnswers { Answer = "I value functionality over aesthetics." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Employee Satisfaction Survey",
                        Description = "Internal survey for company staff",
                        Category = "Human Resources",
                        CreatedAt = DateTime.UtcNow.AddDays(-5).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = false,
                        CompanyID = companyList[4].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "I appreciate the flexibility in working hours." },
                            new SurveyAnswers { Answer = "More training opportunities would be helpful." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Event Feedback Survey",
                        Description = "Share your thoughts about our recent conference",
                        Category = "Events",
                        CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[5].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "The keynote speaker was very inspiring." },
                            new SurveyAnswers { Answer = "The sessions were informative, but the venue was crowded." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Customer Loyalty Survey",
                        Description = "We value our customers' loyalty. Share your experience!",
                        Category = "Customer Service",
                        CreatedAt = DateTime.UtcNow.AddDays(-6).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[0].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "I've been a customer for years because of your great support." },
                            new SurveyAnswers { Answer = "Occasional promotions keep me coming back." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Technology Adoption Survey",
                        Description = "We want to learn about your technology usage patterns",
                        Category = "Technology",
                        CreatedAt = DateTime.UtcNow.AddDays(-22).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[1].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "I adopt new technologies as soon as they are available." },
                            new SurveyAnswers { Answer = "I prefer to wait and see before trying new tools." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Market Trends Survey",
                        Description = "Help us understand the latest market trends",
                        Category = "Market Research",
                        CreatedAt = DateTime.UtcNow.AddDays(-18).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[3].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "The market is moving toward sustainability and eco-friendly products." },
                            new SurveyAnswers { Answer = "Digital transformation is reshaping industries." }
                        }
                    },
                    new Survey.Survey.Survey
                    {
                        Title = "Employee Engagement Survey",
                        Description = "We want to know how engaged our employees feel at work",
                        Category = "Human Resources",
                        CreatedAt = DateTime.UtcNow.AddDays(-4).AddHours(-rand.Next(1, 12)).AddMinutes(-rand.Next(1, 60)),
                        IsActive = true,
                        CompanyID = companyList[2].CompanyID,
                        SurveyAnswers = new List<SurveyAnswers>
                        {
                            new SurveyAnswers { Answer = "I feel valued and supported in my role." },
                            new SurveyAnswers { Answer = "The company offers great career development opportunities." }
                        }
                    }
                };

                context.Surveys.AddRange(surveys);
                context.SaveChanges();
            }
        }
    }
}
