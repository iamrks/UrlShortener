using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PingController(IConfiguration configuration, ILogger<PingController> logger)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"{_configuration.GetValue<string>("Environment")}");
        }
    }
}
