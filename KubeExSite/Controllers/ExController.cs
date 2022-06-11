using Microsoft.AspNetCore.Mvc;

namespace KubeExSite.Controllers
{
    /// <summary>
    /// For testing request to another app service
    /// </summary>
    [ApiController]
    [Route("api/ex")]
    public class ExController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ExController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("gen-list")]
        public IActionResult GenList()
        {
            var testList = new List<string>() { "Py", "C#", "Ruby", "Java" };

            return Ok(testList);
        }
    }
}
