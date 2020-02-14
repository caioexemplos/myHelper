using System.ComponentModel.DataAnnotations;

namespace AuthApp.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Email please")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password please")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}