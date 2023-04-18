using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;
using static MyRecipes.DataAccess.Models.Recipe;
using Recipe = MyRecipes.DataAccess.Models.Recipe;

namespace MyRecipes.Pages.Recipes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly IHtmlHelper htmlHelper;
        private readonly IIngredientData ingredientData;
        private readonly IImageData imageData;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly UserManager<ApplicationUser> userManager;

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public IFormFile NewImage { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }
        public IEnumerable<SelectListItem> Difficulty { get; set; }
        public CreateModel(IRecipeData recipeData, IHtmlHelper htmlHelper, 
                           IIngredientData ingredientData, 
                           UserManager<ApplicationUser> userManager,
                           IImageData imageData,
                           IWebHostEnvironment hostEnvironment)
        {
            this.recipeData = recipeData;
            this.htmlHelper = htmlHelper;
            this.ingredientData = ingredientData;
            this.imageData = imageData;
            this.userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult OnGet(int? recipeId)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            Difficulty = htmlHelper.GetEnumSelectList<DifficultyType>();
            Recipe = new Recipe();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                Difficulty = htmlHelper.GetEnumSelectList<DifficultyType>();
                return Page();    
            }

            if (NewImage != null)
            {
                imageData.SetServerPath(_hostEnvironment.WebRootPath);
                imageData.SetWebPath(HttpContext.Request.GetEncodedUrl().Replace(HttpContext.Request.Path.Value ?? string.Empty, string.Empty));
                var image = await imageData.UploadImageAsync(NewImage);
                Recipe.ImageUrl = image;
            }

            Recipe.UserId = userId;
            recipeData.Add(Recipe);
            TempData["Message"] = "Recipe Added.";
            recipeData.Commit();
            return RedirectToPage("./Detail", new { recipeId = Recipe.Id});
        }
    }
}
