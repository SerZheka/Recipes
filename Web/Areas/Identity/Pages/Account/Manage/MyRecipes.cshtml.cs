using System.Collections.Generic;
using MyRecipes.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class MyRecipesModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IRecipeData recipeData;

        private readonly UserManager<ApplicationUser> userManager;
        public string Message { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }

        public List<int> CurrentMenu { get; set; }
        public string CurrentUserId { get; set; }

        public MyRecipesModel(IConfiguration config,
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
            Recipes = recipeData.GetUserRecipes(CurrentUserId);

            CurrentMenu = HttpContext.Session.GetObjectFromJson<List<int>>("CurrentMenu") ?? new List<int>();
        }
    }
}