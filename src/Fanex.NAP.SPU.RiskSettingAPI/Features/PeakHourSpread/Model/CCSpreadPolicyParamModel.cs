namespace Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread
{
    public class CCSpreadPolicyParamModel
    {       
        public bool IsTest { get; set; }
       
        public bool IsLive { get; set; }
     
        public bool IsLicensee { get; set; }
     
        public DateTimeOffset RepUpdateTime { get; set; }
      
        public long RowNum { get; set; }
       
        public int MaxRecords { get; set; }
    }
}
