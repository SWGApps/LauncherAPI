namespace LauncherAPI.Models.Properties
{
    public class CreateAccountCredentials
    {
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Email { get; set; }
        public string Captcha { get; set; }
    }
}