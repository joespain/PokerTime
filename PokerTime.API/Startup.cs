using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokerTime.API.Data;
using PokerTime.Shared.Converters;
using PokerTime.Shared.Email;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace PokerTime.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PTContext>();
            services.AddScoped<IPTRepository, PTRepository>();

            services.Configure<MailSettings>(_configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                    {
                        //policy.WithOrigins("https://pokertimeapp.azurewebsites.net")
                        policy.WithOrigins(_configuration.GetValue<string>("AppAddress"))
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter()))
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter()));

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer",
                options =>
                {
                    //options.Authority = "https://ptidentityserver.azurewebsites.net";
                    options.Authority = _configuration.GetValue<string>("IDPAddress");
                    options.Audience = "PokerTimeApi";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("api-access", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api-access");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            //app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseCors("default");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
