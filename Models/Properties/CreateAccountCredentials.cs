namespace LauncherAPI.Models.Properties
{
    public class CreateAccountCredentials
    {
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Email { get; set; }
        public string Discord { get; set; }
        public string CaptchaValue1 { get; set; }
        public string CaptchaValue2 { get; set; }
        public string CaptchaAnswer { get; set; }
        public string SubscribeToNewsletter { get; set; }
    }
}