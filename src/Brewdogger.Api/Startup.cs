using AutoMapper;
using Brewdogger.Api.Persistence;
using Brewdogger.Api.Repositories;
using Brewdogger.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brewdogger.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddDbContext<BrewdoggerDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            
            
            // Services and repositories to the IoC container
            services.AddScoped<IBeerRepository, BeerRepository>();
            services.AddScoped<IBreweryRepository, BreweryRepository>();
            services.AddScoped<IBeerService, BeerService>();
            services.AddScoped<IBreweryService, BreweryService>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                // Ignore child elements referring to parents when loading breweries and beers
                .AddJsonOptions(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
