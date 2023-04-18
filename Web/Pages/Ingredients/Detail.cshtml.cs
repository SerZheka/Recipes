using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRecipes.DataAccess.Interfaces;
using Ingredient = MyRecipes.DataAccess.Models.Ingredient;

namespace MyRecipes.Pages.Ingredients
{
    public class DetailModel : PageModel
    {
        private readonly IIngredientData ingredientData;

        public Ingredient Ingredient { get; set; }

        public DetailModel(IIngredientData ingredientData)
        {
            this.ingredientData = ingredientData;
        }

        public IActionResult OnGet(int IngredientId)
        {
            Ingredient = ingredientData.GetById(IngredientId);
            if (Ingredient == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}


