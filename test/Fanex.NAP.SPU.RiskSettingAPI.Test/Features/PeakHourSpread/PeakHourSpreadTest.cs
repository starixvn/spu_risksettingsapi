namespace Fanex.NAP.SPU.RiskSettingAPI.Test.Features.PeakHourSpread
{
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using NSubstitute;

    public class PeakHourSpreadTest
    {
        private readonly IPeakHourSpreadService _peakHourSpreadService;
        private readonly IPeakHourSpreadDataAccess _peakHourSpreadDataAccess;

        public PeakHourSpreadTest()
        {
            _peakHourSpreadDataAccess = Substitute.For<IPeakHourSpreadDataAccess>();
            _peakHourSpreadService = new PeakHourSpreadService(_peakHourSpreadDataAccess);            
        }

        [Fact]
        public void GetCCPeakHourRange_WhenSuccess()
        {
            // Arrange
            var isTest = true;
            var expectedResult = new List<CCPeakHourRangeEntity>
            {
                new CCPeakHourRangeEntity
                {
                    WeekDay = 1,
                    StartRange = new TimeSpan(9, 0, 0),  // 9:00 AM
                    EndRange = new TimeSpan(17, 0, 0)    // 5:00 PM
                },
                new CCPeakHourRangeEntity
                {
                    WeekDay = 2,
                    StartRange = new TimeSpan(10, 0, 0), // 10:00 AM
                    EndRange = new TimeSpan(18, 0, 0)    // 6:00 PM
                }
            };

            _peakHourSpreadDataAccess.GetCCPeakHourRange(isTest).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadService.GetCCPeakHourRange(isTest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count, result.Count());
            Assert.Equal(expectedResult[0].WeekDay, result.First().WeekDay);
            Assert.Equal(expectedResult[0].StartRange, result.First().StartRange);
            Assert.Equal(expectedResult[0].EndRange, result.First().EndRange);
            Assert.Equal(expectedResult[1].WeekDay, result.Last().WeekDay);
            Assert.Equal(expectedResult[1].StartRange, result.Last().StartRange);
            Assert.Equal(expectedResult[1].EndRange, result.Last().EndRange);
        }

        [Fact]
        public void GetCCPeakHourRange_ThrowException()
        {
            // Arrange
            var isTest = true;
            var expectedException = new Exception("Database connection error");
            _peakHourSpreadDataAccess.GetCCPeakHourRange(isTest).Returns(x => throw expectedException);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _peakHourSpreadService.GetCCPeakHourRange(isTest));
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public void GetCCPeakHourDiff_WhenSuccess()
        {
            // Arrange
            var parameters = new CCSpreadPolicyParamModel
            {
                IsTest = true,
                IsLive = true,
                IsLicensee = false,
                RepUpdateTime = DateTime.Now,
                RowNum = 1,
                MaxRecords = 100
            };

            var isWriteLogInfo = true;

            var expectedResult = new CCSpreadPolicyModel
            {
                SpreadPolicy = new List<CCSpreadPolicyEntity>
                {
                    new CCSpreadPolicyEntity
                    {
                        PolicyID = 1,
                        Action = "Update",
                        RepUpdateTime = DateTime.Now
                    }
                },
                SpreadPolicyDetail = new List<CCSpreadPolicyDetailEntity>
                {
                    new CCSpreadPolicyDetailEntity
                    {
                        PolicyID = 1,
                        CustomerClass = 1,
                        SiteID = 1,
                        Currency = 1,
                        Platform = 1,
                        SportType = 1,
                        LeagueGroupID = 1,
                        Spread = 1.5m,
                        IsLive = true,
                        RepUpdateTime = DateTime.Now,
                        RowNum = 1
                    }
                }
            };

            _peakHourSpreadDataAccess.GetCCPeakHourDiff(parameters, isWriteLogInfo).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadService.GetCCPeakHourDiff(parameters, isWriteLogInfo);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SpreadPolicy);
            Assert.NotNull(result.SpreadPolicyDetail);
            Assert.Single(result.SpreadPolicy);
            Assert.Single(result.SpreadPolicyDetail);

            var policy = result.SpreadPolicy.First();
            Assert.Equal(expectedResult.SpreadPolicy.First().PolicyID, policy.PolicyID);
            Assert.Equal(expectedResult.SpreadPolicy.First().Action, policy.Action);
            Assert.Equal(expectedResult.SpreadPolicy.First().RepUpdateTime, policy.RepUpdateTime);

            var policyDetail = result.SpreadPolicyDetail.First();
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().PolicyID, policyDetail.PolicyID);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().CustomerClass, policyDetail.CustomerClass);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().SiteID, policyDetail.SiteID);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().Currency, policyDetail.Currency);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().Platform, policyDetail.Platform);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().SportType, policyDetail.SportType);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().LeagueGroupID, policyDetail.LeagueGroupID);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().Spread, policyDetail.Spread);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().IsLive, policyDetail.IsLive);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().RepUpdateTime, policyDetail.RepUpdateTime);
            Assert.Equal(expectedResult.SpreadPolicyDetail.First().RowNum, policyDetail.RowNum);
        }

        [Fact]
        public void GetCCPeakHourDiff_ThrowException()
        {
            // Arrange
            var parameters = new CCSpreadPolicyParamModel
            {
                IsTest = true,
                IsLive = true,
                IsLicensee = false,
                RepUpdateTime = DateTime.Now,
                RowNum = 1,
                MaxRecords = 100
            };

            var isWriteLogInfo = true;
            var expectedException = new Exception("Database connection error");
            _peakHourSpreadDataAccess.GetCCPeakHourDiff(parameters, isWriteLogInfo).Returns(x => throw expectedException);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _peakHourSpreadService.GetCCPeakHourDiff(parameters, isWriteLogInfo));
            Assert.Equal(expectedException.Message, exception.Message);
        }
    }
}
