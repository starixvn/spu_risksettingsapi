namespace Fanex.NAP.SPU.RiskSettingAPI.Test.Features.PeakHourSpread
{
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using Fanex.NAP.SPU.RiskSettingAPI.JobService;
    using Fanex.NAP.SPU.RiskSettingAPI.JobService.Services.PeakHourSpread;
    using NSubstitute;
    using Xunit;

    public class PeakHourSpreadJobServiceTest
    {
        private readonly IPeakHourSpreadJobService _peakHourSpreadJobService;
        private readonly IPeakHourSpreadService _peakHourSpreadService;
        private readonly IAppSettings _appSettings;

        public PeakHourSpreadJobServiceTest()
        {
            _peakHourSpreadService = Substitute.For<IPeakHourSpreadService>();
            _appSettings = Substitute.For<IAppSettings>();
            _peakHourSpreadJobService = new PeakHourSpreadJobService(_appSettings, _peakHourSpreadService);
        }

        [Fact]
        public void GetCustomerClassPeakHourRange_WhenSuccess_ShouldReturnExpectedData()
        {
            // Arrange
            var isTest = true;
            var expectedResult = new List<CCPeakHourRangeEntity>
            {
                new CCPeakHourRangeEntity
                {
                    WeekDay = 1,
                    StartRange = new TimeSpan(9, 0, 0),
                    EndRange = new TimeSpan(17, 0, 0)
                }
            };

            _peakHourSpreadService.GetCCPeakHourRange(isTest).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadJobService.GetCustomerClassPeakHourRange(isTest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count, result.Count());
            Assert.Equal(expectedResult.First().WeekDay, result.First().WeekDay);
        }
       
        [Fact]
        public void GetCustomerClassPeakHourDiff_WhenSuccess_ShouldReturnExpectedData()
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

            _appSettings.IsWriteLogInfo.Returns(true);

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
                SpreadPolicyDetail = new List<CCSpreadPolicyDetailEntity>()
            };

            _peakHourSpreadService.GetCCPeakHourDiff(parameters, true).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadJobService.GetCustomerClassPeakHourDiff(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SpreadPolicy);
            Assert.Single(result.SpreadPolicy);
            Assert.Equal(expectedResult.SpreadPolicy.First().PolicyID, result.SpreadPolicy.First().PolicyID);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetCustomerClassPeakHourRange_WithDifferentIsTestValues_ShouldCallServiceCorrectly(bool isTest)
        {
            // Arrange
            var expectedResult = new List<CCPeakHourRangeEntity>();
            _peakHourSpreadService.GetCCPeakHourRange(isTest).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadJobService.GetCustomerClassPeakHourRange(isTest);

            // Assert
            _peakHourSpreadService.Received(1).GetCCPeakHourRange(isTest);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetCustomerClassPeakHourDiff_WithDifferentIsWriteLogInfoValues_ShouldCallServiceWithCorrectParameter(bool isWriteLogInfo)
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

            _appSettings.IsWriteLogInfo.Returns(isWriteLogInfo);
            var expectedResult = new CCSpreadPolicyModel();
            _peakHourSpreadService.GetCCPeakHourDiff(parameters, isWriteLogInfo).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadJobService.GetCustomerClassPeakHourDiff(parameters);

            // Assert
            _peakHourSpreadService.Received(1).GetCCPeakHourDiff(parameters, isWriteLogInfo);
            Assert.Equal(expectedResult, result);
        }
    }
} 