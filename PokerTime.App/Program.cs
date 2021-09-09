using BlazorStrap;
using BlazorStyled;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Plk.Blazor.DragDrop;
using PokerTime.App.Data;
using PokerTime.App.Interfaces;
using PokerTime.App.Services;
using System;
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
            builder.Services.AddTransient<TimerService>();
            builder.Services.AddBootstrapCss();
            builder.Services.AddBlazorStyled();


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

            builder.Services.AddHttpClient<ITournamentTrackingDataService, TournamentTrackingDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44328");
            }); //This Http client does not have the Custom Auth Message Handler as it needs to be accessed by users who are not logged in.
            //.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IEmailDataService, EmailDataService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44328");
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();


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
