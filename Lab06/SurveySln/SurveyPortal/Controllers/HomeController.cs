using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyPortal.Models.Survey;
using SurveyPortal.Models.Survey.Survey;
using SurveyPortal.Models.ViewModels;

namespace SurveyPortal.Controllers
{
    public class HomeController : Controller
    {
        private ISurveyRepository _repository;
        public const int PageSize = 4;

        public HomeController(ISurveyRepository repository)
        {
            _repository = repository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Survey model)
        {
            if (!ModelState.IsValid)
            {
               return View(model);
            }
            model.CreatedAt = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
            model.Title = model.Title.Trim();
            model.Description = model.Description.Trim();
            model.Category = model.Category?.Trim();
            model.IsActive = true;

            _repository.AddSurvey(model);

            return RedirectToAction("Details", new { id = model.SurveyID });
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var companies = _repository.Companies
                .Select(c => new SelectListItem
                {
                    Value = c.CompanyID.ToString(),
                    Text = c.Name
                }).ToList();

            ViewBag.Companies = companies;
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(long id)
        {
            var survey = _repository.Surveys.FirstOrDefault(s => s.SurveyID == id);
            if (survey == null)
            {
                return NotFound();
            }
            _repository.DeleteSurvey(survey);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Survey model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var existingSurvey = _repository.Surveys.FirstOrDefault(s => s.SurveyID == model.SurveyID);


            if (existingSurvey == null)
            {
                return NotFound();
            }

            existingSurvey.Title = model.Title;
            existingSurvey.Description = model.Description;
            existingSurvey.Category = model.Category;
            existingSurvey.IsActive = model.IsActive;
            existingSurvey.CreatedAt = DateTime.UtcNow;

            _repository.SaveSurvey(existingSurvey);

            return RedirectToAction("Details", new { id = model.SurveyID });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(long id)
        {
            var survey = _repository.Surveys.FirstOrDefault(s => s.SurveyID == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        private void PopulateNavigationData()
        {
            var categories = _repository.Surveys
                .Select(x => x.Category)
                .Where(x => x != null)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var companies = _repository.Companies
                .Select(x => new { x.CompanyID, x.Name })
                .OrderBy(x => x.Name)
                .ToList();

            ViewBag.Categories = categories;
            ViewBag.Companies = companies;

            ViewBag.SelectedCategory = RouteData?.Values["category"] ?? Request.Query["category"].ToString();
            ViewBag.SelectedCompany = RouteData?.Values["company"] ?? Request.Query["company"].ToString();
        }

        public IActionResult Index(string? category, long? company, int page = 1)
        {
            IQueryable<Survey> query = _repository.Surveys.Include(s => s.Company);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(s => s.Category == category);
            }

            if (company.HasValue)
            {
                query = query.Where(s => s.CompanyID == company.Value);
            }

            var model = new SurveyListViewModel
            {
                Surveys = query
                    .OrderByDescending(s => s.CreatedAt)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = query.Count()
                },
                CurrentCategory = category,
                CurrentCompany = company
            };

            PopulateNavigationData();

            return View(model);
        }
        [Authorize]
        public IActionResult Details(long id)
        {
            var survey = _repository.Surveys
                .Include(s => s.Company)
                .FirstOrDefault(s => s.SurveyID == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }
        [HttpPost]
        [Authorize]
        public IActionResult SaveSurveyAnswer(long SurveyID, string Answer)
        {
            var survey = _repository.Surveys.FirstOrDefault(s => s.SurveyID == SurveyID);

            if (survey == null || string.IsNullOrWhiteSpace(Answer))
            {
                return NotFound();
            }

            _repository.AddAnswer(new SurveyAnswers
            {
                SurveyID = SurveyID,
                Answer = Answer,
                AnsweredAt = DateTime.UtcNow
            });

            TempData["Message"] = "Thank you for your answer!";
            return RedirectToAction("Details", new { id = SurveyID });
        }

        public IActionResult SaveDraftAnswer(long surveyId, [FromBody] DraftDto dto)
        {
            var key = $"draft_{surveyId}";
            HttpContext.Session.SetString(key, dto.Answer ?? "");

            return Ok();
        }

        public class DraftDto
        {
            public string? Answer { get; set; }
        }
    }

}
