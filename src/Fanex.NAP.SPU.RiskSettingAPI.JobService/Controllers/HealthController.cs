namespace Fanex.NAP.SPU.RiskSettingAPI.JobService.Controllers
{
    using Fanex.Logging;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IAppSettings _appSettings;

        public HealthController(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Get()
        {
            LogService.Info($"Check Health Get info is success at {DateTimeOffset.Now}");
            LogService.Error($"Check Health Get error is success at {DateTimeOffset.Now}");

            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Environment = _appSettings.EvironmentName,
                Application = "RiskSettingAPI"
            });
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            LogService.Info($"Check Health Ping info is success at {DateTimeOffset.Now}");
            LogService.Error($"Check Health Ping error is success at {DateTimeOffset.Now}");

            return Ok("pong");
        }
    }
}
