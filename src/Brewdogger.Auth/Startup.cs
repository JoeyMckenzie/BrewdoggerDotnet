using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Brewdogger.Auth.Helpers;
using Brewdogger.Auth.Persistence;
using Brewdogger.Auth.Repositories;
using Brewdogger.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Brewdogger.Auth
{
    public class Startup
    {
        private string _authSecrets = null;
            
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _authSecrets = env.IsDevelopment() ? Configuration["BrewdoggerSecrets:Environment:Development"] : Configuration["BrewdoggerSecrets:Environment:Production"] ;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var secret = _authSecrets = Configuration["BrewdoggerSecrets:Environment:Development"];

            if (string.IsNullOrWhiteSpace(secret))
            {
                // TODO: Need to figure out why secret is null
                Log.Error("Startup::ConfigureServices - Provider secret is null");
                secret = "s8mEegXAQTXmIKCD4vxlsmRpOMjC+cGDkYyDwR453vY=";
            }
            
            Log.Information("Startup::ConfigureServices - Provider secret: {0}", secret);
            
            var jwtSecret = Encoding.ASCII.GetBytes(secret);
            services.AddTransient<ISecretHelper>(_ => new SecretHelper(secret));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            
            // Add Postgres
            services.AddAutoMapper();
            services.AddDbContext<BrewdoggerAuthDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            
            // JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetUserById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            Log.Error("Startup::ConfigureServices - User with userId [{0}] no longer exists", userId);
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            // Configure CORS
            app.UseCors(options => options
                    .AllowAnyOrigin()
                    .WithMethods()
                    .AllowAnyHeader()
                    .AllowCredentials());
            
            // Use JWT auth from above
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
