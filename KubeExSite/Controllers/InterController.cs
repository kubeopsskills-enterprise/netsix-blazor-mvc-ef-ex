using Microsoft.AspNetCore.Mvc;

namespace KubeExSite.Controllers;

/// <summary>
/// For testing request to another app service
/// </summary>
[ApiController]
[Route("api/Inter")]
public class InterController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public InterController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> CallApi()
    {
        var url = _configuration.GetValue<string>("AppToCallUrl");

        HttpClient client = new HttpClient();
        var result = await client.GetStringAsync(url + "/api/Values");
        return Ok(string.IsNullOrEmpty(result) ? "NotFound" : result);
    }
}