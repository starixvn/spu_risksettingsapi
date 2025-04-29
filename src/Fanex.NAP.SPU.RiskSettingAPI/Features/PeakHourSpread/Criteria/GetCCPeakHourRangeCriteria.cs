namespace Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCCPeakHourRangeCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "IsTest")]
        public bool IsTest { get; set; }      

        public override string GetSettingKey()
        {
            return "Dot_Member_CustomerClassPeakHourRange";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
