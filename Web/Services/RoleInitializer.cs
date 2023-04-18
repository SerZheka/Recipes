using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.Services
{
    public class RoleInitializer
    {
        private static string _adminEmail = "admin@gmail.com";
        private static string _password = "_Aa123456";

        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));
            if (await roleManager.FindByNameAsync("user") == null)
                await roleManager.CreateAsync(new IdentityRole("user"));

            if (await userManager.FindByNameAsync(_adminEmail) == null)
            {
                var admin = new ApplicationUser { Email = _adminEmail, UserName = _adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, _password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}