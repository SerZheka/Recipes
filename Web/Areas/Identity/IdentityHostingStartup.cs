using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MyRecipes.Areas.Identity.IdentityHostingStartup))]
namespace MyRecipes.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}