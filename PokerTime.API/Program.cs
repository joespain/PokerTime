using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace PokerTime.API
{
    public class Program
    {
        //public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
        //    .AddEnvironmentVariables()
        //    .Build();

        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .WriteTo.File(new JsonFormatter(), path: @"c:\temp\logs\pokertimeapi-log.json", shared: true)
            //    .CreateLogger();

            try
            {
                Log.Information(messageTemplate: "Starting web host.");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception e)
            {
                Log.Fatal(e, messageTemplate: "Host terminated unexpectedly.");
            }
            //finally
            //{
            //    Log.CloseAndFlush();
            //}
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => logging.AddAzureWebAppDiagnostics())
            .UseStartup<Startup>();
    }
}
