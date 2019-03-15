using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using JPPQUIZ21.Data;
using JPPQUIZ21.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;

namespace JPPQUIZ21
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
            services.AddMvc();

            services.AddEntityFrameworkSqlServer();
            //services.AddDbContext<ApplicationDbContext>(op => op.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<JPPQUIZ21.Data.ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // In production, the Angular files will be served from this directory
            // Add ASP.NET Identity support

            try
            {
                services.AddIdentity<JPPQUIZ21.Data.Models.ApplicationUser, IdentityRole>(
           opts =>
           {
               opts.Password.RequireDigit = true;
               opts.Password.RequireLowercase = true;
               opts.Password.RequireUppercase = true;
               opts.Password.RequireNonAlphanumeric = false;
               opts.Password.RequiredLength = 7;
           })
           .AddEntityFrameworkStores<JPPQUIZ21.Data.ApplicationDbContext>();
            }
            catch (Exception ex)
            {

                var exmessage = ex.Message;
            }

            // Add Authentication with JWT Tokens
            try
            { 

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    // standard configuration
                    ValidIssuer = Configuration["Auth:Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"])),
                    ValidAudience = Configuration["Auth:Jwt:Audience"],
                    ClockSkew = System.TimeSpan.Zero,

                    // security switches
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true
                };
                cfg.IncludeErrorDetails = true;
            });
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

        }
            catch (Exception ex)
            {

                var exmessage = ex.Message;
            }

          
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
           // Add the AuthenticationMiddleware to the pipeline
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });



            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            // Add ASP.NET Identity support
            
            // DbSeeder.Seed(dbContext);
            // Create a service scope to get an ApplicationDbContext instance using DI
            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetService<JPPQUIZ21.Data.ApplicationDbContext>();
            //    // Create the Db if it doesn't exist and applies any pending migration.
            //    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            //    var userManager =serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            //    dbContext.Database.Migrate();
            //    // Seed the Db.
            //    JPPQUIZ21.Data.DbSeeder.Seed(dbContext,roleManager, userManager);
            //}
        }
    }
}
