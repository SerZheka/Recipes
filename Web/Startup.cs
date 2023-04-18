using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyRecipes.DataAccess;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using MyRecipes.DataAccess.Sql;

namespace MyRecipes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<MyRecipesDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RecipesDb"));
                // options.UseSqlServer(Configuration.GetConnectionString("RecipesDbDevelopment"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<MyRecipesDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IRecipeData, SqlRecipeData>();
            services.AddScoped<IIngredientData, SqlIngredientData>();
            services.AddScoped<IFavoriteRecipeData, SqlFavoriteRecipeData>();
            services.AddScoped<IImageData, SqlImageData>();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddDistributedMemoryCache();
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@".\test.keys"));

            services.AddSession(options =>
            {
                // We want to keep the user session active for as long as possible
                options.IdleTimeout = TimeSpan.MaxValue;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Remove character requirements from passwords
            // See: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-5.0#password
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            // for aspnetcore3.0+
            services.AddControllers();
            services.AddMvc();
            
            services.AddAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNodeModules();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });

            // for aspnetcore3.0+
            app.UseEndpoints(e =>
            {
                e.MapRazorPages();
                e.MapControllers();
            });
        }
    }
}
