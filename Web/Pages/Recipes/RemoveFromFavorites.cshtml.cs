using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.Pages.Recipes
{
    public class RemoveFromFavoritesModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFavoriteRecipeData favoriteRecipeData;

        public RemoveFromFavoritesModel(IRecipeData recipeData,
                                   UserManager<ApplicationUser> userManager,
                                   IFavoriteRecipeData favoriteRecipeData)
        {
            this.recipeData = recipeData;
            this.userManager = userManager;
            this.favoriteRecipeData = favoriteRecipeData;
        }

        public async Task<ActionResult> OnPostAsync(int recipeId)
        {
            var recipe = recipeData.GetById(recipeId);
            var user = await userManager.GetUserAsync(User);

            favoriteRecipeData.RemoveFromFavorites(recipe, user);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
