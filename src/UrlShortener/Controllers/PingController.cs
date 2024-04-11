using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services.FeatureFlag;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PingController> _logger;
        private readonly IFeatureFlagService _featureFlagService;

        public PingController(
            IConfiguration configuration,
            ILogger<PingController> logger,
            IFeatureFlagService featureFlagService)
        {
            _configuration = configuration;
            _logger = logger;
            _featureFlagService = featureFlagService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"{_configuration.GetValue<string>("Environment")}");
        }

        [HttpGet("CheckFlag")]
        public IActionResult CheckFlag()
        {
            var flagValue = _featureFlagService.IsFeatureEnabled(FeatureFlagKeys.TestNewFeatureKey, false);

            if (flagValue)
            {
                return Ok("Flag is ON");
            }
            else
            {
                return Ok("Flag is OFF");
            }
        }
    }
}
