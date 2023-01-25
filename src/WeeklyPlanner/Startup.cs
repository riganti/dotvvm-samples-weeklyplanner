using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Core.Common;
using DotVVM.Framework.Api.Swashbuckle.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WeeklyPlanner.Data;
using WeeklyPlanner.DTO;
using WeeklyPlanner.Services;
using WeeklyPlanner.Controllers;
using Swashbuckle.Swagger;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WeeklyPlanner
{
    public class Startup
    {

        public Startup(IWebHostEnvironment env)
        {
            // Set up configuration sources.
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddAuthorization();
            services.AddWebEncoders();
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignedOutRedirectUri = Configuration["AzureAd:PostLogoutRedirectUri"];
                options.ClientId = Configuration["AzureAD:ClientId"];
                options.Authority = Configuration["AzureAd:AadInstance"];
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnRemoteFailure = OnAuthenticationFailed
                };
            });

            services.AddDotVVM<DotvvmStartup>();

            services.AddMvc().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                jsonOptions.JsonSerializerOptions.Converters.Add( new DotvvmApiDateTimeConverter());
            });

            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.Configure<DotvvmApiOptions>(options =>
            {
                options.AddKnownAssembly(typeof(DayViewDTO).Assembly);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v4", new OpenApiInfo { Title = "Weekly Planner API", Version = "v4" });

                c.EnableDotvvmIntegration();
            });

            services.AddEntityFrameworkSqlite()
                .AddDbContext<AppDbContext>();

            services.AddScoped<UserService>();
            services.AddScoped<ScheduledTaskService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseAuthentication();

            // use MVC
            app.UseMvc();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v4/swagger.json", "Weekly Planner API V4");
            });

            // use DotVVM
            app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);

            // use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(env.WebRootPath)
            });
        }

        private Task OnAuthenticationFailed(RemoteFailureContext context)
        {
            context.HandleResponse();
            context.Response.Redirect("/error");
            return Task.FromResult(0);
        }
    }
}
