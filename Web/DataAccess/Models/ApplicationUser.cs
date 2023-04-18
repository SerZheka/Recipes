using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MyRecipes.DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public IEnumerable<FavoriteRecipe> FavoriteRecipes { get; set; }
    }
}
