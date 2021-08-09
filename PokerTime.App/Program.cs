using AutoMapper;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PokerTime.App.Data;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokerTime.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddSingleton<AppDataService>();
            builder.Services.AddAutoMapper(typeof(EntityProfile));

            //Add Data Services
            builder.Services.AddHttpClient<IBlindLevelDataService, BlindLevelDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44328");
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IEventDataService, EventDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44328");
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IHostDataService, HostDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44328");
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IInviteeDataService, InviteeDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44328");
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IStructureDataService, StructureDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44328");
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                options.ProviderOptions.Authority = "https://localhost:5001";
                options.ProviderOptions.ClientId = "wasmappauth-client";
                options.ProviderOptions.ResponseType = "code";
                options.ProviderOptions.DefaultScopes.Add("api-access");
            });



            await builder.Build().RunAsync();
        }
    }
}
