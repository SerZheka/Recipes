using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRecipes.DataAccess.Interfaces;
using Ingredient = MyRecipes.DataAccess.Models.Ingredient;

namespace MyRecipes.Pages.Ingredients
{
    public class ListModel : PageModel
    {
        private readonly IIngredientData ingredientData;

        public IEnumerable<Ingredient> Ingredients { get; set; }

        public ListModel(IIngredientData ingredientData)
        {
 
            this.ingredientData = ingredientData;
        }
        public void OnGet(string searchTerm)
        {
            Ingredients = ingredientData.GetIngredientsByName(searchTerm);
        }
    }
}