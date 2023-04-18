using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.DataAccess.Interfaces
{
    public interface IImageData
    {
        void SetServerPath(string path);
        void SetWebPath(string path);
        Image UploadImage(IFormFile newImage);
        Task<Uri> UploadImageAsync(IFormFile newImageFile);
        Image GetById(int id);
    }
}
