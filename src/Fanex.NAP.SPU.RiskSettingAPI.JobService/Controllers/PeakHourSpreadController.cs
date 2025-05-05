namespace Fanex.NAP.SPU.RiskSettingAPI.JobService.Controllers
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using Fanex.NAP.SPU.RiskSettingAPI.JobService.Services.PeakHourSpread;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/PeakHourSpread")]
    [ApiController]
    public class PeakHourSpreadController : ControllerBase
    {       
        private readonly IPeakHourSpreadJobService _peakHourSpreadJobService;

        public PeakHourSpreadController(IPeakHourSpreadJobService peakHourSpreadJobService)
        {          
            _peakHourSpreadJobService = peakHourSpreadJobService;
        }

        [HttpGet("health")]
        public string CheckHealth()
        {
            LogService.Info($"Check Health info is running at {DateTimeOffset.Now}");
            LogService.Error($"Check Health error is running at {DateTimeOffset.Now}");

            return JsonConvert.SerializeObject(DateTimeOffset.Now);
        }

        [HttpPost("GetCustomerClassPeakHourDiff")]
        public async Task<CCSpreadPolicyModel> GetCustomerClassPeakHourDiff([FromBody] CCSpreadPolicyParamModel paras)
        {
            return await Task.FromResult(_peakHourSpreadJobService.GetCustomerClassPeakHourDiff(paras));
        }

        [HttpPost("GetCustomerClassPeakHourRange")]
        public async Task<IEnumerable<CCPeakHourRangeEntity>> GetCustomerClassPeakHourRange([FromBody] bool isTest)
        {            
            return await Task.FromResult(_peakHourSpreadJobService.GetCustomerClassPeakHourRange(isTest));
        }
    }
}
