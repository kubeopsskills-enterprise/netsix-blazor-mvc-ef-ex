using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KubeExSite.Context;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;
conf.AddJsonFile("appsetting.custom.json", true);
var vaultUrl = conf.GetValue<string>("KeyVaultUrl");

builder.Services.AddApplicationInsightsTelemetry();
if (!string.IsNullOrEmpty(vaultUrl))
{
    try
    {
        conf.AddAzureKeyVault(new Uri(vaultUrl), new DefaultAzureCredential());
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
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
