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

            builder.Services.AddScoped(sp =>
                new HttpClient { BaseAddress = new Uri("http://localhost:6600") });

            //builder.Services.AddHttpClient(IUserDataService, UserDataService)(client =>
            //    client.BaseAddress = new Uri("https://localhost:6600"));

            builder.Services.AddScoped<IUserDataService, UserDataService>();

            await builder.Build().RunAsync();
        }
    }
}
