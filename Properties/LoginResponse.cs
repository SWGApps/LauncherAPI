using System.Collections.Generic;

namespace LauncherAPI.Properties
{
    public class Response
    {
        public string Result { get; set; }
        public string Username { get; set; }
        public List<string> Characters { get; set; }
    }
}