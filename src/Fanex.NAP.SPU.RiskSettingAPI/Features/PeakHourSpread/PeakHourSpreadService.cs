namespace Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread
{   
    public interface IPeakHourSpreadService
    {
        IEnumerable<CCPeakHourRangeEntity> GetCCPeakHourRange(bool isTest);

        CCSpreadPolicyModel GetCCPeakHourDiff(CCSpreadPolicyParamModel paras);
    }

    public class PeakHourSpreadService : IPeakHourSpreadService
    {
        private readonly IPeakHourSpreadDataAccess _dataAccess;

        public PeakHourSpreadService(IPeakHourSpreadDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<CCPeakHourRangeEntity> GetCCPeakHourRange(bool isTest)
        {
            return _dataAccess.GetCCPeakHourRange(isTest);
        }

        public CCSpreadPolicyModel GetCCPeakHourDiff(CCSpreadPolicyParamModel paras)
        {
            return _dataAccess.GetCCPeakHourDiff(paras);
        }
    }
}
