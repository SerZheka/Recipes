using Microsoft.AspNetCore.Mvc;
using MyRecipes.DataAccess.Interfaces;

namespace MyRecipes.Pages.ViewComponents
{
    public class RecipeCountViewComponent
        : ViewComponent
    {
        private readonly IRecipeData recipeData;

        public RecipeCountViewComponent(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
        }

        public IViewComponentResult Invoke()
        {
            var count = recipeData.GetCountOfRecipes();
            return View(count);
        }
    }

    

}
