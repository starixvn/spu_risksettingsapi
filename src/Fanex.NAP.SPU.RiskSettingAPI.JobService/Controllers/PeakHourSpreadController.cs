namespace Fanex.NAP.SPU.RiskSettingAPI.JobService.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/PeakHourSpread")]
    [ApiController]
    public class PeakHourSpreadController : ControllerBase
    {
        private readonly ISentryClient _sentryClient;

        public PeakHourSpreadController(ISentryClient sentryClient)
        {
            _sentryClient = sentryClient;
        }

        [HttpPost("CustomerClassPeakHourDiff")]
        public async Task<bool> CustomerClassPeakHourDiff()
        {
            return await Task.FromResult(true);
        }

        [HttpPost("CustomerClassPeakHourRange")]
        public async Task<bool> CustomerClassPeakHourRange()
        {
            return await Task.FromResult(true);
        }
    }
}
