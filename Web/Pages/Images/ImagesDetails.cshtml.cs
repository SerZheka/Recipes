using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.Pages.Images
{
    public class ImagesDetailsModel : PageModel
    {
        private readonly IImageData imageData;

        public Image Image { get; set; }

        public ImagesDetailsModel(IImageData imageData)
        {
            this.imageData = imageData;
        }

        public IActionResult OnGet(int imageId)
        {
            Image image = imageData.GetById(imageId);      
            return File(image.ImageData, "image/jpeg");
        }
    }
}
