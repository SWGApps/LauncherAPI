using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LauncherAPI.Models.Properties;
using LauncherAPI.Models;

namespace LauncherAPI.Controllers
{
    [ApiController]
    [Route("/account/[controller]")]
    public class CreateController : ControllerBase
    {
        readonly ILogger<CreateController> _logger;
        APIQuery _query;

        public CreateController(ILogger<CreateController> logger)
        {
            _logger = logger;
            _query = new APIQuery();
        }

        [HttpPost("{Username}")]
        public string Post(string Username, [FromForm] CreateAccountCredentials credentials)
        {
            return _query.Create(Username, credentials);
        }

        [HttpPost("")]
        public string Post()
        {
            return _query.CreateEmpty();
        }
    }
}
