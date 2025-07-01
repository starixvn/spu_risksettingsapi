namespace Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread
{
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Newtonsoft.Json;

    public interface IPeakHourSpreadDataAccess
    {
        IEnumerable<CCPeakHourRangeEntity> GetCCPeakHourRange(bool isTest);

        CCSpreadPolicyModel GetCCPeakHourDiff(CCSpreadPolicyParamModel paras, bool isWriteLogInfo);
    }

    public class PeakHourSpreadDataAccess : IPeakHourSpreadDataAccess
    {
        private readonly IDynamicRepository _repository;

        public PeakHourSpreadDataAccess() : this(new DynamicRepository())
        {
        }

        public PeakHourSpreadDataAccess(IDynamicRepository repository)
        {
            _repository = repository;
        }   

        public IEnumerable<CCPeakHourRangeEntity> GetCCPeakHourRange(bool isTest)
        {
            var criteria = new GetCCPeakHourRangeCriteria
            {
                IsTest = isTest
            };

            return _repository.Fetch<CCPeakHourRangeEntity>(criteria);
        }

        public CCSpreadPolicyModel GetCCPeakHourDiff(CCSpreadPolicyParamModel paras, bool isWriteLogInfo)
        {
            if (isWriteLogInfo)
            {
                LogService.Info($"Get CC Peak Hour Diff - before execute SP at time: {DateTimeOffset.Now} with paras {JsonConvert.SerializeObject(paras)}");
            }

            var criteria = new GetCCPeakHourDiffCriteria
            {
                IsTest = paras.IsTest,
                IsLive = paras.IsLive,
                IsLicensee = paras.IsLicensee,
                RepUpdateTime = paras.RepUpdateTime,
                RowNum = paras.RowNum,
                MaxRecords = paras.MaxRecords
            };

            var (spreadPolicy, spreadPolicyDetail) = _repository.FetchMultiple<CCSpreadPolicyEntity, CCSpreadPolicyDetailEntity>(criteria);

            if (isWriteLogInfo)
            {
                LogService.Info($"Get CC Peak Hour Diff - after execute SP at time: {DateTimeOffset.Now} with paras {JsonConvert.SerializeObject(paras)}");
            }
            
            return new CCSpreadPolicyModel
            {
                SpreadPolicy = spreadPolicy,
                SpreadPolicyDetail = spreadPolicyDetail
            };
        }
    }
}
