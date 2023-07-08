namespace WebApplication1.Models
{
    public class LoginRegisterModel
    {
        public SignInModel SignIn { get; set; } = new SignInModel();
        public RegisterModel Register { get; set; } = new RegisterModel();
    }
}
