namespace Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;  

    public class GetCCPeakHourDiffCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "IsTest")]
        public bool IsTest { get; set; }

        [SpParam(SqlParamName = "Islive")]
        public bool IsLive { get; set; }

        [SpParam(SqlParamName = "IsLicensee")]
        public bool IsLicensee { get; set; }

        [SpParam(SqlParamName = "RepUpdateTime")]
        public DateTimeOffset RepUpdateTime { get; set; }

        [SpParam(SqlParamName = "RowNum")]
        public long RowNum { get; set; }

        [SpParam(SqlParamName = "maxRecords")]
        public int MaxRecords { get; set; }

        public override string GetSettingKey()
        {
            return "Dot_Member_CustomerClassPeakHourDiff";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
