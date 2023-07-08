using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RegisterModel
    {
        [StringLength(100, MinimumLength = 5)]
        [Required(ErrorMessage = "Please insert username")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please insert nickname")]
        public string Nickname { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please insert password")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please insert email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please insert phone number")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
