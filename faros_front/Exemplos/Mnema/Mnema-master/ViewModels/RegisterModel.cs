using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AuthApp.ViewModels
{
    public class RegisterModel
    {

        [Required(ErrorMessage ="Enter Name please")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Enter Email please")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password please")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Invalid password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Upload Avatar please")]
        public IFormFile Avatar { get; set; }
    }
}