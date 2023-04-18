using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRecipes.DataAccess.Interfaces;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.Pages.Recipes
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IRecipeData recipeData;

        public Recipe Recipe { get; set; }

        public DeleteModel(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
        }
        public IActionResult OnGet(int recipeId)
        {
            Recipe = recipeData.GetById(recipeId);
            if(Recipe == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost(int recipeId)
        {
            var recipe = recipeData.Delete(recipeId);
            recipeData.Commit();

            if(recipe == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"The recipe {recipe.Name} has been deleted.";
            return RedirectToPage("./List");


        }
    }
}
