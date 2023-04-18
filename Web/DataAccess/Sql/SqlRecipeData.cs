using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.DataAccess.Sql
{
    public class SqlRecipeData : IRecipeData
    {
        private readonly MyRecipesDbContext db;

        public SqlRecipeData(MyRecipesDbContext db)
        {
            this.db = db;
        }
        public Recipe Add(Recipe newRecipe)
        {
            db.Add(newRecipe);
            return newRecipe;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Recipe Delete(int id)
        {
            var recipe = GetById(id);
            if(recipe !=null)
            {
                db.Recipes.Remove(recipe);
            }
            return recipe;
        }

        public Recipe GetById(int id)
        {
            return db.Recipes.Where(r => r.Id == id)
                             .Include(r => r.Ingredients)
                             .Include(r => r.Steps)
                             .AsNoTracking()
                             .First();
        }

        public int GetCountOfRecipes()
        {
            return db.Recipes.Count();
        }

        public IEnumerable<Recipe> GetUserRecipes(string userId)
        {
            return db.Recipes.Where(r => r.UserId == userId).AsNoTracking();
        }

        public IEnumerable<FavoriteRecipe> GetFavouriteRecipesByName(string name, string userId)
        {
            return db.FavoriteRecipes
                .Where(r => r.UserId == userId)
                .Include(r => r.Recipe)
                .ThenInclude(r => r.User)
                .Where(r => r.Recipe.Name.StartsWith(name) || string.IsNullOrEmpty(name))
                .OrderBy(r => r.Recipe.Name)
                .AsNoTracking();
        }

        public IEnumerable<Recipe> GetRecipesByIds(IEnumerable<int> ids)
        {
            var query = from r in db.Recipes.Include(r => r.Ingredients)
                        where ids.Contains(r.Id)
                        orderby r.Name
                        select r;
            return query;
        }

        public IEnumerable<Recipe> GetRecipesByName(string name)
        {
            var query = from r in db.Recipes.Include(r=> r.Ingredients)
                        where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Recipe Update(Recipe updatedRecipe)
        {
            var entity = db.Recipes.Attach(updatedRecipe);
            entity.State = EntityState.Modified;
            // Prevent user Id from changing
            entity.Property(x => x.UserId).IsModified = false;
            return updatedRecipe;
        }

    }
}