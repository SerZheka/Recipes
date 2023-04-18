using System.Collections.Generic;
using MyRecipes.DataAccess.Models;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.DataAccess.Interfaces
{
    public interface IRecipeData
    {
        IEnumerable<Recipe> GetRecipesByName(string name);
        IEnumerable<Recipe> GetUserRecipes(string userId);
        IEnumerable<FavoriteRecipe> GetFavouriteRecipesByName(string name, string userId);
        IEnumerable<Recipe> GetRecipesByIds(IEnumerable<int> ids);
        Recipe GetById(int id);
        Recipe Update(Recipe updatedRecipe);
        Recipe Add(Recipe newRecipe);
        Recipe Delete(int id);
        int GetCountOfRecipes();
        int Commit();

    }
}