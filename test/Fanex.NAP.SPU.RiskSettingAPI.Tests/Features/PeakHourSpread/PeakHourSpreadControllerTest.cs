namespace Fanex.NAP.SPU.RiskSettingAPI.Test.Features.PeakHourSpread
{
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using Fanex.NAP.SPU.RiskSettingAPI.JobService.Controllers;
    using Fanex.NAP.SPU.RiskSettingAPI.JobService.Services.PeakHourSpread;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;

    public class PeakHourSpreadControllerTest
    {
        private readonly PeakHourSpreadController _controller;
        private readonly IPeakHourSpreadJobService _peakHourSpreadJobService;

        public PeakHourSpreadControllerTest()
        {
            _peakHourSpreadJobService = Substitute.For<IPeakHourSpreadJobService>();
            _controller = new PeakHourSpreadController(_peakHourSpreadJobService);
        }        

        [Fact]
        public async Task GetCustomerClassPeakHourRange_WhenSuccess_ShouldReturnExpectedData()
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

            _peakHourSpreadJobService.GetCustomerClassPeakHourRange(isTest).Returns(expectedResult);

            // Act
            var result = await _controller.GetCustomerClassPeakHourRange(isTest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count, result.Count());
            Assert.Equal(expectedResult.First().WeekDay, result.First().WeekDay);
        }

        [Fact]
        public async Task GetCustomerClassPeakHourRange_WhenServiceReturnsEmpty_ShouldReturnEmptyCollection()
        {
            // Arrange
            var isTest = false;
            var expectedResult = new List<CCPeakHourRangeEntity>();
            _peakHourSpreadJobService.GetCustomerClassPeakHourRange(isTest).Returns(expectedResult);

            // Act
            var result = await _controller.GetCustomerClassPeakHourRange(isTest);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCustomerClassPeakHourDiff_WhenSuccess_ShouldReturnExpectedData()
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

            _peakHourSpreadJobService.GetCustomerClassPeakHourDiff(parameters).Returns(expectedResult);

            // Act
            var result = await _controller.GetCustomerClassPeakHourDiff(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SpreadPolicy);
            Assert.NotNull(result.SpreadPolicyDetail);
            Assert.Single(result.SpreadPolicy);
            Assert.Single(result.SpreadPolicyDetail);
            Assert.Equal(expectedResult.SpreadPolicy.First().PolicyID, result.SpreadPolicy.First().PolicyID);
        }

        [Fact]
        public async Task GetCustomerClassPeakHourDiff_WhenServiceReturnsEmptyModel_ShouldReturnEmptyModel()
        {
            // Arrange
            var parameters = new CCSpreadPolicyParamModel
            {
                IsTest = false,
                IsLive = false,
                IsLicensee = true,
                RepUpdateTime = DateTime.Now,
                RowNum = 10,
                MaxRecords = 50
            };

            var expectedResult = new CCSpreadPolicyModel();
            _peakHourSpreadJobService.GetCustomerClassPeakHourDiff(parameters).Returns(expectedResult);

            // Act
            var result = await _controller.GetCustomerClassPeakHourDiff(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SpreadPolicy);
            Assert.NotNull(result.SpreadPolicyDetail);
            Assert.Empty(result.SpreadPolicy);
            Assert.Empty(result.SpreadPolicyDetail);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetCustomerClassPeakHourRange_WithDifferentIsTestValues_ShouldCallServiceCorrectly(bool isTest)
        {
            // Arrange
            var expectedResult = new List<CCPeakHourRangeEntity>();
            _peakHourSpreadJobService.GetCustomerClassPeakHourRange(isTest).Returns(expectedResult);

            // Act
            var result = await _controller.GetCustomerClassPeakHourRange(isTest);

            // Assert
            _peakHourSpreadJobService.Received(1).GetCustomerClassPeakHourRange(isTest);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetCustomerClassPeakHourDiff_WithNullParameters_ShouldHandleGracefully()
        {
            // Arrange
            CCSpreadPolicyParamModel parameters = null;
            var expectedResult = new CCSpreadPolicyModel();
            _peakHourSpreadJobService.GetCustomerClassPeakHourDiff(parameters).Returns(expectedResult);

            // Act
            var result = await _controller.GetCustomerClassPeakHourDiff(parameters);

            // Assert
            _peakHourSpreadJobService.Received(1).GetCustomerClassPeakHourDiff(parameters);
            Assert.Equal(expectedResult, result);
        }
    }
} 