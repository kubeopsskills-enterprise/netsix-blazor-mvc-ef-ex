using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KubeExSite.Context;
using KubeExSite.InjectServices;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;
conf.AddJsonFile("appsetting.custom.json", true);
var vaultUrl = conf.GetValue<string>("KeyVaultUrl");

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddRedisService(conf);
if (!string.IsNullOrEmpty(vaultUrl))
{
    try
    {
        conf.AddAzureKeyVault(new Uri(vaultUrl),new DefaultAzureCredential());
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(
    options =>
    {
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
builder.Services.AddRazorPages();
builder.Services.AddDbContext<KubeExContext>(optionsBuilder =>
{
    var conn = conf.GetConnectionString("Default");
    if (string.IsNullOrEmpty(conn))
    {
        conn = conf.GetValue<string>(conf.GetValue<string>("DbConnectionKey"));
    }
    var connectionBuilder = new NpgsqlConnectionStringBuilder(conn);
    //connectionBuilder.SslMode = SslMode.VerifyFull;
    optionsBuilder.EnableDetailedErrors();

    //optionsBuilder.UseNpgsql(connectionBuilder.ConnectionString).UseSnakeCaseNamingConvention();
    optionsBuilder.UseInMemoryDatabase("kube_ex").UseSnakeCaseNamingConvention();
});

var app = builder.Build();


if (!conf.GetValue<string>("BaseUrl").IsNullOrEmpty())
{
    app.UsePathBase(conf.GetValue<string>("BaseUrl"));
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapHealthChecks("/healthz");
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapBlazorHub();
//     endpoints.MapControllers();
//     endpoints.MapHealthChecks("/healthz");
// });

app.Run();
