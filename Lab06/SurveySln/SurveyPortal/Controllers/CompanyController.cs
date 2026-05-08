using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyPortal.Models.Company;
using SurveyPortal.Models.Survey;
using SurveyPortal.Models.Survey.Survey;

namespace SurveyPortal.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ISurveyRepository _repository;
        public const int PageSize = 8;

        public CompaniesController(ISurveyRepository repository)
        {
            _repository = repository;
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
        public IActionResult Index()
        {
            var companies = _repository.Companies
                .OrderBy(c => c.Name)
                .ToList();

            PopulateNavigationData();

            return View(companies);
        }

        public IActionResult Details(long id)
        {
            var company = _repository.Companies
                .Include(c => c.Surveys)
                .FirstOrDefault(c => c.CompanyID == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                company.CreatedAt = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond));
                company.Name = company.Name.Trim();
                company.Description = company.Description?.Trim();
                company.Country = company.Country?.Trim();

                _repository.AddCompany(company);
                return RedirectToAction(nameof(Details), new { id = company.CompanyID });
            }

            return View(company);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Edit(long id)
        {
            var company = _repository.Companies.FirstOrDefault(c => c.CompanyID == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                var existingCompany = _repository.Companies.FirstOrDefault(c => c.CompanyID == company.CompanyID);

                if (existingCompany == null)
                {
                    return NotFound();
                }

                existingCompany.Name = company.Name.Trim();
                existingCompany.Description = company.Description?.Trim();
                existingCompany.Country = company.Country?.Trim();

                _repository.SaveCompany(existingCompany);
                return RedirectToAction(nameof(Details), new { id = company.CompanyID });
            }

            return View(company);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long id)
        {
            var company = _repository.Companies
                .Include(c => c.Surveys)
                .FirstOrDefault(c => c.CompanyID == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var company = _repository.Companies.FirstOrDefault(c => c.CompanyID == id);

            if (company == null)
            {
                return NotFound();
            }

            _repository.DeleteCompany(company);
            return RedirectToAction(nameof(Index));
        }
    }
}