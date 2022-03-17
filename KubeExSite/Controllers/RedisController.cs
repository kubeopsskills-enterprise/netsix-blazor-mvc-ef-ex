using KubeExSite.DTOs;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace KubeExSite.Controllers;


[Route("api/redis")]
[ApiController]
public class RedisController : ControllerBase
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisController(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }
    [HttpGet]
    public async Task<IActionResult> GetCache()
    {
        var redisEndPoint = _connectionMultiplexer.GetEndPoints().FirstOrDefault();
        var redisServ = _connectionMultiplexer.GetServer(redisEndPoint);

        List<TagDto> strList = new List<TagDto>();
        await foreach (var key in redisServ.KeysAsync(1 ,pattern: "ex_*"))
        {
            var db = _connectionMultiplexer.GetDatabase(1);
            var value = await db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                strList.Add(new TagDto(){Key = key, Value = value});
            }
        }
        
        return Ok(strList);
    }

    [HttpPost]
    public async Task<IActionResult> SetCache([FromBody]TagDto tagDto)
    {
        var db = _connectionMultiplexer.GetDatabase(1);

        await db.StringSetAsync("ex_"+tagDto.Key, tagDto.Value);

        return Ok();
    }
}