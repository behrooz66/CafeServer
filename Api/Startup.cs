using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AuthServer.Repositories;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Api.Helpers;

namespace Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddTransient<IAuthRepository, AuthRepository>()
                .AddTransient<IProvinceRepository, ProvinceRepository>()
                .AddTransient<ICountryRepository, CountryRepository>()
                .AddTransient<ICityRepository, CityRepository>()
                .AddTransient<ICustomerRepository, CustomerRepository>()
                .AddTransient<IGiftCardRepository, GiftCardRepository>()
                .AddTransient<IGiftCardTypeRepository, GiftCardTypeRepository>()
                .AddTransient<IOrderRepository, OrderRepository>()
                .AddTransient<IOrderTypeRepository, OrderTypeRepository>()
                .AddTransient<IReservationRepository, ReservationRepository>()
                .AddTransient<IReservationStatusRepository, ReservationStatusRepository>()
                .AddTransient<IRestaurantRepository, RestaurantRepository>()
                .AddTransient<IHelper, Helper>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                //Authority = "http://localhost:5000",
                Authority = Configuration.GetSection("BD").GetSection("Authority").Value,
                ApiName = "api",
                RequireHttpsMetadata = false
            });

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowCredentials();
            }); 
            app.UseMvc();
        }
    }
}
