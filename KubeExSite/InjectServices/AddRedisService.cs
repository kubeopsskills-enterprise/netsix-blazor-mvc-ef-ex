using StackExchange.Redis;

namespace KubeExSite.InjectServices;

public static class AddCacheService
{
    public static void AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        var conStr = configuration.GetValue<string>("Redis:ConnectionStr");

        if (conStr != null)
        {
            try
            {
                var redisConStr = ConfigurationOptions.Parse(conStr);
                redisConStr.ConnectRetry = 30;
                
                var redis = ConnectionMultiplexer.Connect(redisConStr);
                if (redis.IsConnected)
                {
                    services.AddSingleton<IConnectionMultiplexer>(redis);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}