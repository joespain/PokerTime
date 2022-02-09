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
        public static void Main(string[] args)
        {
            try
            {
                Log.Information(messageTemplate: "Starting web host.");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch(Exception e)
            {
                Log.Fatal(e, messageTemplate: "Host terminated unexpectedly.");
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => logging.AddAzureWebAppDiagnostics())
            .UseStartup<Startup>();
    }
}
