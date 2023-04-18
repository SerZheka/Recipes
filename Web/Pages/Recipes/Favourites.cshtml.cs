using System.Collections.Generic;
using MyRecipes.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.Pages.Recipes
{
    public class Favourites : PageModel
    {
        private readonly IConfiguration config;
        private readonly IRecipeData recipeData;

        private readonly UserManager<ApplicationUser> userManager;
        public string Message { get; set; }
        public IEnumerable<FavoriteRecipe> Recipes { get; set; }

        public List<int> CurrentMenu { get; set; }
        public string CurrentUserId { get; set; }

        public Favourites(IConfiguration config,
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
            Recipes = recipeData.GetFavouriteRecipesByName(searchTerm, CurrentUserId);

            CurrentMenu = HttpContext.Session.GetObjectFromJson<List<int>>("CurrentMenu") ?? new List<int>();
        }

    }
}