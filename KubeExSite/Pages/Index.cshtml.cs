using KubeExSite.Context;
using KubeExSite.Context.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KubeExSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly KubeExContext _dbContext;
        public IndexModel(ILogger<IndexModel> logger, KubeExContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult OnGet()
        {
            ViewData["routine_reports"] = _dbContext.RoutineReports.AsQueryable().Take(100).ToList();

            return Page();
        }
    }
}