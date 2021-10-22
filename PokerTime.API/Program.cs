using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using System;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

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
            .ConfigureServices(serviceCollection => serviceCollection.Configure<AzureFileLoggerOptions>(options =>
                {
                    options.FileName = "azure-diagnostics-";
                    options.FileSizeLimit = 50 * 1024;
                    options.RetainedFileCountLimit = 5;
                })
                .Configure<AzureBlobLoggerOptions>(options =>
                {
                    options.BlobName = "log.txt";
                }))
            .UseStartup<Startup>();
    }
}
