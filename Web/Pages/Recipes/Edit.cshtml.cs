using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class EditModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly IHtmlHelper htmlHelper;
        private readonly IIngredientData ingredientData;
        private readonly IImageData imageData;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public IFormFile NewImage { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }
        public IEnumerable<SelectListItem> Difficulty { get; set; }
        public EditModel(IRecipeData recipeData,
                         IHtmlHelper htmlHelper, 
                         IIngredientData ingredientData, 
                         IImageData imageData,
                         UserManager<ApplicationUser> userManager,
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
            Recipe = recipeData.GetById(recipeId.Value);

            // Authorizing the user to edit their own recipes
            var currentUserId = userManager.GetUserId(User); 
            var authorId = Recipe.UserId;

            if (currentUserId != authorId && !User.IsInRole("admin")) 
                return RedirectToPage("./List");
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            Difficulty = htmlHelper.GetEnumSelectList<DifficultyType>();
            if (Recipe == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();    
            }

            if (NewImage != null)
            {
                imageData.SetServerPath(_hostEnvironment.WebRootPath);
                imageData.SetWebPath(HttpContext.Request.GetEncodedUrl().Replace(HttpContext.Request.Path.Value ?? string.Empty, string.Empty));
                var image = await imageData.UploadImageAsync(NewImage);
                Recipe.ImageUrl = image;
            }

            // Update existing Ingredients
            var savedIngredients = new List<int>();
            foreach (var ingredient in Recipe.Ingredients)
            {
                if (ingredient.Id > 0)
                {
                    savedIngredients.Add(ingredient.Id);
                    ingredientData.Update(ingredient);
                }
            }

            // Delete removed ingredients
            var originalRecipe = recipeData.GetById(Recipe.Id);
            var deletedIngredients = originalRecipe.Ingredients.Where(i => !savedIngredients.Contains(i.Id));
            foreach (var deletedIngredient in deletedIngredients)
            {
                ingredientData.Delete(deletedIngredient.Id);
            }    
        
            // Update Recipe
            recipeData.Update(Recipe);
            recipeData.Commit();
            TempData["Message"] = "Recipe Updated.";
            return RedirectToPage("./Detail", new { recipeId = Recipe.Id});
        }
    }
}
