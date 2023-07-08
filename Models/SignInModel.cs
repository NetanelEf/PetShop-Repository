using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SignInModel
    {
        [StringLength(100,MinimumLength =5)]
        [Required(ErrorMessage = "Please insert username")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please insert password")]
        public string Password { get; set; } = string.Empty;
    }
}
