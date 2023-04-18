using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using MyRecipes.Helpers;

namespace MyRecipes.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "admin")]
    public class UserRecipes : PageModel
    {
        private readonly IConfiguration config;
        private readonly IRecipeData recipeData;

        private readonly UserManager<ApplicationUser> userManager;
        public string Message { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }

        public List<int> CurrentMenu { get; set; }
        public string CurrentUserId { get; set; }

        public UserRecipes(IConfiguration config,
            IRecipeData recipeData,
            UserManager<ApplicationUser> userManager)
        {
            this.recipeData = recipeData;
            this.userManager = userManager;
            this.config = config;
        }

        public async Task<IActionResult> OnGet(string UserId, string searchTerm)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
                return NotFound($"User not found '{UserId}'.");

            CurrentUserId = UserId;
            Message = config["Message"];
            Recipes = recipeData.GetUserRecipes(CurrentUserId);

            CurrentMenu = HttpContext.Session.GetObjectFromJson<List<int>>("CurrentMenu") ?? new List<int>();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int recipeId)
        {
            var recipe = recipeData.Delete(recipeId);
            recipeData.Commit();

            if (recipe == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"The recipe {recipe.Name} has been deleted.";
            return Page();
        }
    }
}