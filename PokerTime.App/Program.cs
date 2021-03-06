using BlazorStrap;
using BlazorStyled;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
            });

            builder.Services.AddTransient<CustomAuthorizationMessageHandler>();
            builder.Services.AddSingleton<AppDataService>();
            builder.Services.AddAutoMapper(typeof(EntityProfile));
            builder.Services.AddTransient<TimerService>();
            builder.Services.AddBootstrapCss();
            builder.Services.AddBlazorStyled();

            builder.Services.AddHostedService<TimerService>();

            //API address
            Uri pokerTimeApi = new(builder.Configuration.GetValue<string>("ApiAddress"));

            //Add Data Services
            builder.Services.AddHttpClient<IBlindLevelDataService, BlindLevelDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IEventDataService, EventDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IHostDataService, HostDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IInviteeDataService, InviteeDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IStructureDataService, StructureDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<ITournamentTrackingDataService, TournamentTrackingDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            });
                //The CustomMessageHandler is not included in TournamentTracking to allow anonymous users
                //.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<ITournamentEventDataService, TournamentEventDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<IEmailDataService, EmailDataService>(client =>
            {
                client.BaseAddress = pokerTimeApi;
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            await builder.Build().RunAsync();
        }
    }
}
