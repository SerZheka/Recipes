using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using MyRecipes.Helpers;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.Pages.Recipes
{
    public class DetailModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFavoriteRecipeData favoriteRecipeData;

        [TempData]
        public string Message { get; set; }

        public Recipe Recipe { get; set; }

        public List<int> CurrentMenu { get; set; }

        public string CurrentUserId { get; set; }

        public bool IsFavorited { get; set; }
        

        public DetailModel(IRecipeData recipeData,
                           UserManager<ApplicationUser> userManager,
                           IFavoriteRecipeData favoriteRecipeData)
        {
            this.recipeData = recipeData;
            this.userManager = userManager;
            this.favoriteRecipeData = favoriteRecipeData;
        }
        public async System.Threading.Tasks.Task<IActionResult> OnGetAsync(int recipeId)
        {
            // Get the recipe
            Recipe = recipeData.GetById(recipeId);
            if (Recipe == null)
            {
                return RedirectToPage("./NotFound");
            }

            // Get Current Menu for Cookbook 
            CurrentMenu = HttpContext.Session.GetObjectFromJson<List<int>>("CurrentMenu");
            if (CurrentMenu == null)
            {
                CurrentMenu = new List<int>();
            }

            // Get Current User Id
            CurrentUserId = userManager.GetUserId(User);
            var user = await userManager.GetUserAsync(User);

            // Get favorites status
            IsFavorited = favoriteRecipeData.IsFavorited(Recipe, user);


            // Get Viewed Recipes
            var viewedRecipes = HttpContext.Session.GetObjectFromJson<List<int>>("ViewedRecipesKey");

            if (viewedRecipes == null)
            {
                viewedRecipes = new List<int>();
            }
            viewedRecipes.Add(recipeId);
            HttpContext.Session.SetObjectAsJson("ViewedRecipesKey", viewedRecipes);

            return Page();
        }
    }
}