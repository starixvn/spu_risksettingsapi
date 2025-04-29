namespace Fanex.NAP.SPU.RiskSettingAPI.JobService.Services.PeakHourSpread
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using System;

    public interface IPeakHourSpreadJobService
    {
        IEnumerable<CCPeakHourRangeEntity> GetCustomerClassPeakHourRange(bool isTest);

        CCSpreadPolicyModel GetCustomerClassPeakHourDiff(CCSpreadPolicyParamModel paras);
    }

    public class PeakHourSpreadJobService : IPeakHourSpreadJobService
    {       
        private readonly IAppSettings _appSettings;
        private readonly IPeakHourSpreadService _peakHourSpreadService;

        public PeakHourSpreadJobService(     
            IAppSettings appSettings,
            IPeakHourSpreadService peakHourSpreadService)
        {           
            _appSettings = appSettings;
            _peakHourSpreadService = peakHourSpreadService;
        }

        public IEnumerable<CCPeakHourRangeEntity> GetCustomerClassPeakHourRange(bool isTest)
        {
            try
            {
                //LogService.Info($"Get Customer Class Peak Hour Range is running at {DateTime.Now}");
              
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
                //LogService.Info($"Get Customer Class Peak Diff Range is running at {DateTime.Now}");
              
                return _peakHourSpreadService.GetCCPeakHourDiff(paras);
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
