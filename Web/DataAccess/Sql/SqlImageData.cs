using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.DataAccess.Sql
{
    public class SqlImageData : IImageData
    {
        private readonly MyRecipesDbContext db;
        private string _serverPath;
        private string _webPath;

        public SqlImageData(MyRecipesDbContext db)
        {
            this.db = db;
        }

        public async Task<Uri> UploadImageAsync(IFormFile newImageFile)
        {
            var uploadsFolder = Path.Combine(_serverPath, "images");  
            var fileName = Guid.NewGuid() + "_" + newImageFile.FileName;  
            var filePath = Path.Combine(uploadsFolder, fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))  
            {  
                await newImageFile.CopyToAsync(fileStream);  
            }  
        
            return new Uri($"/images/{fileName}", UriKind.Relative);
        }

        public void SetServerPath(string path) => _serverPath = path;
        public void SetWebPath(string path) => _webPath = path;

        public Image UploadImage(IFormFile newImageFile)
        {
            var newImage = new Image();
            newImage.ImageTitle = newImageFile.FileName;
            var ms = new MemoryStream();
            newImageFile.CopyTo(ms);
            newImage.ImageData = ms.ToArray();
            
            db.Add(newImage);
            db.SaveChanges();
            
            return newImage;
        }

        public Image GetById(int id)
        {
            return db.Images
                .First(i => i.Id == id);
        }
    }
}