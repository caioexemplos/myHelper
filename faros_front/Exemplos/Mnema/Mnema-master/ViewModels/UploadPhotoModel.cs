using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AuthApp.ViewModels
{
    public class UploadPhotoModel
    {
        public IFormFile  uploadedFile{ get; set; }
        public string Description { get; set; }
    }
}