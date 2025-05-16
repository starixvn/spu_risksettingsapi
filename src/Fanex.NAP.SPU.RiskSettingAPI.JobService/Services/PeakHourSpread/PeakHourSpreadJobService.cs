namespace Fanex.NAP.SPU.RiskSettingAPI.JobService.Services.PeakHourSpread
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using System;
    using Newtonsoft.Json;

    public interface IPeakHourSpreadJobService
    {
        IEnumerable<CCPeakHourRangeEntity> GetCustomerClassPeakHourRange(bool isTest);

        CCSpreadPolicyModel GetCustomerClassPeakHourDiff(CCSpreadPolicyParamModel paras);
    }

    public class PeakHourSpreadJobService : IPeakHourSpreadJobService
    {              
        private readonly IPeakHourSpreadService _peakHourSpreadService;
        private readonly IAppSettings _appSettings;

        public PeakHourSpreadJobService(     
            IAppSettings appSettings,
            IPeakHourSpreadService peakHourSpreadService)
        {                     
            _peakHourSpreadService = peakHourSpreadService;
            _appSettings = appSettings;
        }

        public IEnumerable<CCPeakHourRangeEntity> GetCustomerClassPeakHourRange(bool isTest)
        {
            try
            {                             
                return _peakHourSpreadService.GetCCPeakHourRange(isTest);                
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                   .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                   .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                   .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                   .ToString();

                LogService.Error(logMessage);
            }

            return Enumerable.Empty<CCPeakHourRangeEntity>();
        }

        public CCSpreadPolicyModel GetCustomerClassPeakHourDiff(CCSpreadPolicyParamModel paras)
        {
            try
            {
                var result = _peakHourSpreadService.GetCCPeakHourDiff(paras, _appSettings.IsWriteLogInfo);
              
                return result;
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                   .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                   .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                   .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                   .ToString();

                LogService.Error(logMessage);
            }

            return new CCSpreadPolicyModel();
        }
    }
}
