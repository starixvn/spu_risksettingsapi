namespace Fanex.NAP.SPU.RiskSettingAPI.JobService
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions;

    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                
                host.Run();
            }
            catch (Exception ex)
            {
                //var logMessage = new LogMessage(ex.Message)
                //   .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                //   .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                //   .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                //   .ToString();

                //LogService.Error(logMessage);

                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(env.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
#pragma warning disable S1075 // URIs should not be hardcoded
                    webBuilder.UseUrls("http://*:5000");
#pragma warning restore S1075 // URIs should not be hardcoded

                    webBuilder
                    .UseSentry()
                    .UseStartup<Startup>();
                });
        }
    }
}
