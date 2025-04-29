namespace Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread
{   
    public class CCSpreadPolicyModel
    {
        public CCSpreadPolicyModel() 
        {
            SpreadPolicy = new List<CCSpreadPolicyEntity>();
            SpreadPolicyDetail = new List<CCSpreadPolicyDetailEntity>();
        }

        public IEnumerable<CCSpreadPolicyEntity> SpreadPolicy { get; set; }

        public IEnumerable<CCSpreadPolicyDetailEntity> SpreadPolicyDetail { get; set; }
    }
}
