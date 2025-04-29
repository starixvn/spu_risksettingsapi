namespace Fanex.NAP.SPU.RiskSettingAPI.JobService.Controllers
{
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using Fanex.NAP.SPU.RiskSettingAPI.JobService.Services.PeakHourSpread;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/PeakHourSpread")]
    [ApiController]
    public class PeakHourSpreadController : ControllerBase
    {
        //private readonly ISentryClient _sentryClient;
        private readonly IPeakHourSpreadJobService _peakHourSpreadJobService;

        public PeakHourSpreadController(
            IPeakHourSpreadJobService peakHourSpreadJobService)
        {
            //_sentryClient = sentryClient;
            _peakHourSpreadJobService = peakHourSpreadJobService;
        }

        [HttpPost("GetCustomerClassPeakHourDiff")]
        public async Task<CCSpreadPolicyModel> GetCustomerClassPeakHourDiff(CCSpreadPolicyParamModel paras)
        {
            return await Task.FromResult(_peakHourSpreadJobService.GetCustomerClassPeakHourDiff(paras));
        }

        [HttpPost("GetCustomerClassPeakHourRange")]
        public async Task<IEnumerable<CCPeakHourRangeEntity>> GetCustomerClassPeakHourRange(bool isTest)
        {
            return await Task.FromResult(_peakHourSpreadJobService.GetCustomerClassPeakHourRange(isTest));
        }
    }
}
