using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace KubeExSite.Controllers;

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

        Dictionary<string,string> strList = new Dictionary<string, string>();
        await foreach (var key in redisServ.KeysAsync(pattern: "ex_*"))
        {
            for (var i = 0; i < redisServ.DatabaseCount; i++)
            {
                var db = _connectionMultiplexer.GetDatabase(i);
                var value = await db.StringGetAsync(key);
                if (string.IsNullOrEmpty(value))
                {
                    strList.Add(key, value);
                }
            }
        }
        
        return Ok(strList);
    }

    [HttpPost]
    public async Task<IActionResult> SetCache(string key, string value)
    {
        var db = _connectionMultiplexer.GetDatabase(1);

        await db.StringSetAsync(key, value);

        return Ok();
    }
}