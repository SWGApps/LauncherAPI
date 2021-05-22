using System.Collections.Generic;

namespace LauncherAPI.Models.Properties
{
    public class LoginResponse
    {
        public string Result { get; set; }
        public string Username { get; set; }
        public List<string> Characters { get; set; }
    }
}