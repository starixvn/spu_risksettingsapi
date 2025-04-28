namespace Fanex.NAP.SPU.RiskSettingAPI.JobService.Services
{
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using Fanex.NAP.SPU.RiskSettingAPI.JobService.Services.PeakHourSpread;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterPeakHourSpread(services);
        }

        public static void RegisterPeakHourSpread(IServiceCollection services)
        {
            services.AddSingleton<IPeakHourSpreadDataAccess, PeakHourSpreadDataAccess>();
            services.AddSingleton<IPeakHourSpreadService, PeakHourSpreadService>();
            services.AddSingleton<IPeakHourSpreadJobService, PeakHourSpreadJobService>();
        }

    }
}
