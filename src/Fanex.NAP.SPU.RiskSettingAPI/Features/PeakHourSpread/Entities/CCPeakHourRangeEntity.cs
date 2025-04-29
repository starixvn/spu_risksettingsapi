namespace Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread
{
    using System;

    public class CCPeakHourRangeEntity
    {
        public int WeekDay { get; set; }

        public TimeSpan StartRange { get; set; }

        public TimeSpan EndRange { get; set; }
    }
}
