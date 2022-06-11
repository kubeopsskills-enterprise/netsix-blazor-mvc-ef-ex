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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("gen-list")]
        public IActionResult GenList()
        {
            var testList = new List<string>() { "Py", "C#", "Ruby", "Java" };

            return Ok(testList);
        }

        [HttpGet("gen-cookies")]
        public IActionResult GenCookies()
        {
            _httpContextAccessor
                .HttpContext?
                .Response
                .Cookies
                .Append("TestCookie1", "true", new CookieOptions
                {
                    SameSite = SameSiteMode.Lax, Expires = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(5)
                });

            return Ok();
        }
    }
}
