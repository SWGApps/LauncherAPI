using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LauncherAPI.Models.Properties;
using LauncherAPI.Models;

namespace LauncherAPI.Controllers
{
    [ApiController]
    [Route("/account/[controller]")]
    public class LoginController : ControllerBase
    {
        readonly ILogger<LoginController> _logger;
        APIQuery _query;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            _query = new APIQuery();
        }

        [HttpPost("{Username}")]
        public string Post(string Username, [FromForm] LoginCredentials credentials)
        {
            return _query.Login(Username, credentials);
        }

        [HttpPost("")]
        public string Post()
        {
            return _query.LoginEmpty();
        }
    }
}
