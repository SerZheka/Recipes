using System.Linq;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.DataAccess.Sql
{
    public class SqlFavoriteRecipeData : IFavoriteRecipeData
    {
        private readonly MyRecipesDbContext _db;
        public SqlFavoriteRecipeData (MyRecipesDbContext db)
        {
            _db = db;
        }

        public void AddToFavorites(Recipe recipe, ApplicationUser user)
        {
            if(IsFavorited(recipe,user))
                return;

            var favorite = new FavoriteRecipe
            {
                RecipeId = recipe.Id,
                UserId = user.Id
            };
            //favorite.CreatedAt = DateTime.Now;
            _db.Add(favorite);
            _db.SaveChanges();
        }

        public bool IsFavorited(Recipe recipe, ApplicationUser user)
        {
            if(user == null)
            {
                return false;
            }

            var favoriteRecord = _db.FavoriteRecipes
                .FirstOrDefault(f => f.RecipeId == recipe.Id && f.UserId == user.Id);
            return favoriteRecord != null;
        }

        public bool RemoveFromFavorites(Recipe recipe, ApplicationUser user)
        {
            var favorite = _db.FavoriteRecipes
                .First(f => f.RecipeId == recipe.Id && f.UserId == user.Id);
            _db.Remove(favorite);
            _db.SaveChanges();

            return true;
        }

    }

    
}
