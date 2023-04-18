using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using MyRecipes.Helpers;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.Pages.Recipes
{
    [AllowAnonymous]
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IRecipeData recipeData;

        private readonly UserManager<ApplicationUser> userManager;
        public string Message { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }

        public List<int> CurrentMenu { get; set; }
        public IEnumerable<Recipe> ViewedRecipes { get; set; }

        public string CurrentUserId { get; set; }

        public ListModel(IConfiguration config,
                        IRecipeData recipeData,
                        UserManager<ApplicationUser> userManager)
        {
            this.recipeData = recipeData;
            this.userManager = userManager;
            this.config = config;

        }
        public void OnGet(string searchTerm)
        {
            CurrentUserId = userManager.GetUserId(User);
            Message = config["Message"];
            Recipes = recipeData.GetRecipesByName(searchTerm);

            CurrentMenu = HttpContext.Session.GetObjectFromJson<List<int>>("CurrentMenu");
            if (CurrentMenu == null)
            {
                CurrentMenu = new List<int>();
            }
            CurrentMenu.Count();

            //Listing Last five Recipes Seen
            var ViewedRecipesIds = HttpContext.Session.GetObjectFromJson<List<int>>("ViewedRecipesKey");
            if (ViewedRecipesIds == null)
            {
                ViewedRecipesIds = new List<int>();
            }
        
            ViewedRecipes = recipeData.GetRecipesByIds(ViewedRecipesIds.TakeLast(5));
        }
    }
}