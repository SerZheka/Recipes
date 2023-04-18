using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyRecipes.DataAccess.Models;
using Ingredient = MyRecipes.DataAccess.Models.Ingredient;
using Recipe = MyRecipes.DataAccess.Models.Recipe;


namespace MyRecipes.DataAccess
{
    public class MyRecipesDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyRecipesDbContext(DbContextOptions<MyRecipesDbContext> options)
            : base(options)
        {

        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Step> Steps { get; set; }

        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
