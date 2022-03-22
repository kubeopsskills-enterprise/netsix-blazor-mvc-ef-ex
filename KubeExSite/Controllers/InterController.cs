using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        var strList = JsonConvert.DeserializeObject<List<string>>(result);
        strList?.Add(url);
        return Ok(strList == null || strList.Count == 0 ? "NotFound" : strList);
    }
}