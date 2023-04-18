using System.Collections.Generic;
using MyRecipes.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRecipes.Pages.Recipes
{
    public class SaveToMenuModel : PageModel
    {
        

        public ActionResult OnPost(int recipeId)
        {
            var currentMenu = HttpContext.Session.GetObjectFromJson<List<int>>("CurrentMenu");

            if (currentMenu == null)
            {
                currentMenu = new List<int>();
            }
            currentMenu.Add(recipeId);
            HttpContext.Session.SetObjectAsJson("CurrentMenu", currentMenu);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
 }
