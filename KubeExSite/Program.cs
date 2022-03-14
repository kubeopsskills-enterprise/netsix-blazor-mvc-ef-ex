using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KubeExSite.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;
conf.AddJsonFile("appsetting.custom.json", true);
var vaultName = conf.GetValue<string>("KeyVaultName");
builder.Services.AddApplicationInsightsTelemetry();
if (!string.IsNullOrEmpty(vaultName))
{
    try
    {
        conf.AddAzureKeyVault(new Uri($"https://{vaultName}.vault.azure.net"), new DefaultAzureCredential());
    }
    catch (Exception e)
    {
        Console.Error.Write(e.Message);
    }
}

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<KubeExContext>(optionsBuilder =>
{
    var conn = conf.GetConnectionString("Default");
    if (string.IsNullOrEmpty(conn))
    {
        conn = conf.GetValue<string>(conf.GetValue<string>("DbConnectionKey"));
    }
    var connectionBuilder = new NpgsqlConnectionStringBuilder(conn);
    connectionBuilder.SslMode = SslMode.VerifyFull;
    optionsBuilder.EnableDetailedErrors();
    
    optionsBuilder.UseNpgsql(connectionBuilder.ConnectionString).UseSnakeCaseNamingConvention();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseHealthChecks("/healthz");

app.Run();
