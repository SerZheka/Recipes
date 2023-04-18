using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyRecipes.DataAccess.Interfaces;
using Ingredient = MyRecipes.DataAccess.Models.Ingredient;

namespace MyRecipes.DataAccess.Sql
{
    public class SqlIngredientData : IIngredientData
    {
        private readonly MyRecipesDbContext db;

        public SqlIngredientData(MyRecipesDbContext db)
        {
            this.db = db;
        }
        public Ingredient Add(Ingredient newIngredient)
        {
            db.Add(newIngredient);
            return newIngredient;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Ingredient Delete(int id)
        {
            var ingredient = GetById(id);
            if(ingredient !=null)
            {
                db.Ingredients.Remove(ingredient);
            }
            return ingredient;
        }
  
        public Ingredient GetById(int id)
        {
            return db.Ingredients.Find(id);
        }

        public int GetCountOfIngredients()
        {
            return db.Ingredients.Count();
        }

        public IEnumerable<Ingredient> GetIngredientsByName(string name)
        {
            var query = from r in db.Ingredients
                        where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Ingredient Update(Ingredient updatedIngredient)
        {
            var entity = db.Ingredients.Attach(updatedIngredient);
            entity.State = EntityState.Modified;
            return updatedIngredient;
        }

    }
}