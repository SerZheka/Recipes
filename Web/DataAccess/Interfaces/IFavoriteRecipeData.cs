using MyRecipes.DataAccess.Models;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.DataAccess.Interfaces
{
    public interface IFavoriteRecipeData
    {
        void AddToFavorites(Recipe recipe, ApplicationUser user);
        bool RemoveFromFavorites(Recipe recipe, ApplicationUser user);

        bool IsFavorited (Recipe recipe, ApplicationUser user);
    }
}

