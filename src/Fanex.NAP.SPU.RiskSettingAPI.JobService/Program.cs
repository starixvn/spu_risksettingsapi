using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.NAP.SPU.RiskSettingAPI.JobService;
using Fanex.NAP.SPU.RiskSettingAPI.JobService.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Configuration;

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fanex.NAP.SPU.RiskSetting.JobService", Version = "v1" });
});

services.AddHealthChecks();
services.AddSingleton<IAppSettings>(new AppSettings(configuration));
services.RegisterService();
services.AddFanexLogSentry(configuration);

var appSettings = new AppSettings(configuration);

DbSettingProviderManager
.StartNewSession()
.Use(configuration.GetSection("Connections")
            .GetChildren()
            .AsEnumerable()
            .ToDictionary(
                    x => x.GetValue<string>("Name"),
                    y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName"))
            ))
.WithSqlServer()
  .UseDefaultDbSettingProvider(resourcePath: appSettings.SPName)
  .Run();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();