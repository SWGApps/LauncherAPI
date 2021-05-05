using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LauncherAPI.Properties;
using System.Collections.Generic;

namespace LauncherAPI.Controllers
{
    [ApiController]
    [Route("/account/[controller]")]
    public class LoginController : ControllerBase
    {
        readonly ILogger<LoginController> _logger;
        string _dbSecret;
        APIQuery _query;
        SWGUtils _util;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            _dbSecret = "swgemus3cr37!";
            _util = new SWGUtils();
            _query = new APIQuery();
        }

        [HttpPost("{Username}")]
        public string Post(string Username, [FromForm] Credentials credentials)
        {
            // Tuple, from database
            (string password, string salt) = _query.GetUserCredentials(Username);
            
            if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(salt))
            {
                string passwordInput;
                passwordInput = credentials.Password;
                
                string hashedPasswordInput = _util.HashPassword(passwordInput, _dbSecret, salt);

                if (password == hashedPasswordInput.Trim())
                {
                    List<string> characters = _query.GetCharacters(Username);

                    return JsonConvert.SerializeObject(new Response {
                        Result = "Success",
                        Username = Username,
                        Characters = characters
                    }, Formatting.Indented);
                }
            }

            return JsonConvert.SerializeObject(new Response {
                Result = "InvalidCredentials",
                Username = Username,
                Characters = new List<string>()
            }, Formatting.Indented);
        }
    }
}
