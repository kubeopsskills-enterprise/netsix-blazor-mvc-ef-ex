using Microsoft.AspNetCore.Mvc;

namespace KubeExSite.Controllers;

[ApiController]
public class RedisController : ControllerBase
{
    public RedisController()
    {
        
    }
    [HttpGet]
    public IActionResult GetCache()
    {
        return Ok();
    }
}