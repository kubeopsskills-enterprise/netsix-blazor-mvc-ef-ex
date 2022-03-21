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
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, KubeExContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            _logger.Log(LogLevel.Information,"CheckVaultUrl: "+_configuration.GetValue<string>("KeyVaultUrl"));
            _logger.Log(LogLevel.Information,"CheckDbConnectionKey: "+_configuration.GetValue<string>("DbConnectionKey"));
            //ViewData["routine_reports"] = _dbContext.RoutineReports.AsQueryable().OrderBy(report => report.Id).Take(100).ToList();

            return Page();
        }
    }
}