using Microsoft.AspNetCore.Mvc;
using SurveyPortal.Models.Survey.Survey;

namespace SurveyPortal.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {   
        private readonly SurveyDbContext _context;

        public NavigationMenuViewComponent(SurveyDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var categories = _context.Surveys
                .Select(s => s.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return View(categories);
        }
    }
}
