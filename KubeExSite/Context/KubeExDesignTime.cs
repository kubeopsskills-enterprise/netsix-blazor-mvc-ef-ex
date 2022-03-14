using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace KubeExSite.Context;

public class KubeExDesignTime : IDesignTimeDbContextFactory<KubeExContext>
{
    public KubeExContext CreateDbContext(string[] args)
    {
        var configInit = new ConfigurationBuilder()
            .AddJsonFile("DesignTimeSetting.json");
        var configFromFile = configInit.Build();

        var connectionBuilder = new NpgsqlConnectionStringBuilder(configFromFile.GetConnectionString("Default"));
        connectionBuilder.SslMode = SslMode.VerifyFull;

        var builder = new DbContextOptionsBuilder<KubeExContext>();
        builder.EnableDetailedErrors();
        
        builder.UseNpgsql(connectionBuilder.ConnectionString)
            .UseSnakeCaseNamingConvention();

        return new KubeExContext(builder.Options);
    }
}