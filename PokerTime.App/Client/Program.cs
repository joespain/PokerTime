using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PokerTime.App.Client.Interfaces;
using PokerTime.App.Client.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace PokerTime.App.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //HttpClient
            builder.Services.AddScoped(sp =>
                new HttpClient { BaseAddress = new Uri("http://localhost:6600") });

            //Data services that connect to API
            builder.Services.AddScoped<IUserDataService, UserDataService>();
            builder.Services.AddScoped<IStructureDataService, StructureDataService>();
            builder.Services.AddScoped<IBlindLevelDataService, BlindLevelDataService>();

            await builder.Build().RunAsync();
        }
    }
}
