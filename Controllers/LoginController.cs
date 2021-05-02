using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LauncherAPI.Controllers
{
    public class Credentials
    {
        public string Password { get; set; }    
    }

    public class Response
    {
        public string result { get; set; }
        public string username { get; set; }

    }

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
                    Response response = new Response();

                    response.result = "success";
                    response.username = Username;

                    return JsonConvert.SerializeObject(response);
                }
            }

            return $"Details for user: {Username} \n Password: {password} \n Salt: {salt}";
        }
    }
}
