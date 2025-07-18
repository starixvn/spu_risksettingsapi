namespace Fanex.NAP.SPU.RiskSettingAPI.Test.Features.PeakHourSpread
{
    using Fanex.NAP.SPU.RiskSettingAPI.Features.PeakHourSpread;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PeakHourSpreadDataAccessTest
    {
        private readonly PeakHourSpreadDataAccess _peakHourSpreadDataAccess;
        private readonly IDynamicRepository _repository;

        public PeakHourSpreadDataAccessTest()
        {
            _repository = Substitute.For<IDynamicRepository>();
            _peakHourSpreadDataAccess = new PeakHourSpreadDataAccess(_repository);
        }
       
        [Fact]
        public void Constructor_WithValidRepository_ShouldCreateInstance()
        {
            // Act
            var dataAccess = new PeakHourSpreadDataAccess(_repository);

            // Assert
            Assert.NotNull(dataAccess);
        }

        [Fact]
        public void Constructor_WithDefaultConstructor_ShouldCreateInstance()
        {
            // Act
            var dataAccess = new PeakHourSpreadDataAccess();

            // Assert
            Assert.NotNull(dataAccess);
        }

        [Fact]
        public void GetCCPeakHourRange_WhenSuccess_ShouldReturnExpectedData()
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

            _repository.Fetch<CCPeakHourRangeEntity>(Arg.Any<GetCCPeakHourRangeCriteria>()).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadDataAccess.GetCCPeakHourRange(isTest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count, result.Count());
            
            // Verify first item
            var firstItem = result.First();
            Assert.Equal(expectedResult[0].WeekDay, firstItem.WeekDay);
            Assert.Equal(expectedResult[0].StartRange, firstItem.StartRange);
            Assert.Equal(expectedResult[0].EndRange, firstItem.EndRange);
            
            // Verify second item
            var secondItem = result.Last();
            Assert.Equal(expectedResult[1].WeekDay, secondItem.WeekDay);
            Assert.Equal(expectedResult[1].StartRange, secondItem.StartRange);
            Assert.Equal(expectedResult[1].EndRange, secondItem.EndRange);

            // Verify repository was called with correct criteria
            _repository.Received(1).Fetch<CCPeakHourRangeEntity>(Arg.Is<GetCCPeakHourRangeCriteria>(c => c.IsTest == isTest));
        }

        [Fact]
        public void GetCCPeakHourRange_WhenRepositoryReturnsEmpty_ShouldReturnEmptyCollection()
        {
            // Arrange
            var isTest = false;
            var expectedResult = new List<CCPeakHourRangeEntity>();
            _repository.Fetch<CCPeakHourRangeEntity>(Arg.Any<GetCCPeakHourRangeCriteria>()).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadDataAccess.GetCCPeakHourRange(isTest);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _repository.Received(1).Fetch<CCPeakHourRangeEntity>(Arg.Is<GetCCPeakHourRangeCriteria>(c => c.IsTest == isTest));
        }

        [Fact]
        public void GetCCPeakHourRange_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var isTest = true;
            var expectedException = new Exception("Database connection error");
            _repository.Fetch<CCPeakHourRangeEntity>(Arg.Any<GetCCPeakHourRangeCriteria>()).Returns(x => throw expectedException);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _peakHourSpreadDataAccess.GetCCPeakHourRange(isTest));
            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetCCPeakHourRange_WithDifferentIsTestValues_ShouldCallRepositoryWithCorrectCriteria(bool isTest)
        {
            // Arrange
            var expectedResult = new List<CCPeakHourRangeEntity>();
            _repository.Fetch<CCPeakHourRangeEntity>(Arg.Any<GetCCPeakHourRangeCriteria>()).Returns(expectedResult);

            // Act
            var result = _peakHourSpreadDataAccess.GetCCPeakHourRange(isTest);

            // Assert
            _repository.Received(1).Fetch<CCPeakHourRangeEntity>(Arg.Is<GetCCPeakHourRangeCriteria>(c => c.IsTest == isTest));
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void GetCCPeakHourDiff_WhenIsWriteLogInfoFalse_ShouldNotLogAndReturnExpectedData()
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

            var isWriteLogInfo = false;
            var expectedSpreadPolicy = new List<CCSpreadPolicyEntity>();
            var expectedSpreadPolicyDetail = new List<CCSpreadPolicyDetailEntity>();

            _repository.FetchMultiple<CCSpreadPolicyEntity, CCSpreadPolicyDetailEntity>(Arg.Any<GetCCPeakHourDiffCriteria>())
                .Returns((expectedSpreadPolicy, expectedSpreadPolicyDetail));

            // Act
            var result = _peakHourSpreadDataAccess.GetCCPeakHourDiff(parameters, isWriteLogInfo);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SpreadPolicy);
            Assert.NotNull(result.SpreadPolicyDetail);
            Assert.Empty(result.SpreadPolicy);
            Assert.Empty(result.SpreadPolicyDetail);

            // Verify repository was called with correct criteria
            _repository.Received(1).FetchMultiple<CCSpreadPolicyEntity, CCSpreadPolicyDetailEntity>(
                Arg.Is<GetCCPeakHourDiffCriteria>(c => 
                    c.IsTest == parameters.IsTest &&
                    c.IsLive == parameters.IsLive &&
                    c.IsLicensee == parameters.IsLicensee &&
                    c.RepUpdateTime == parameters.RepUpdateTime &&
                    c.RowNum == parameters.RowNum &&
                    c.MaxRecords == parameters.MaxRecords));
        }
       
        [Fact]
        public void GetCCPeakHourDiff_WhenRepositoryReturnsEmptyCollections_ShouldReturnEmptyModel()
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

            var isWriteLogInfo = false;
            var expectedSpreadPolicy = new List<CCSpreadPolicyEntity>();
            var expectedSpreadPolicyDetail = new List<CCSpreadPolicyDetailEntity>();

            _repository.FetchMultiple<CCSpreadPolicyEntity, CCSpreadPolicyDetailEntity>(Arg.Any<GetCCPeakHourDiffCriteria>())
                .Returns((expectedSpreadPolicy, expectedSpreadPolicyDetail));

            // Act
            var result = _peakHourSpreadDataAccess.GetCCPeakHourDiff(parameters, isWriteLogInfo);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SpreadPolicy);
            Assert.NotNull(result.SpreadPolicyDetail);
            Assert.Empty(result.SpreadPolicy);
            Assert.Empty(result.SpreadPolicyDetail);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, false, true)]
        [InlineData(true, false, true)]
        public void GetCCPeakHourDiff_WithDifferentParameterCombinations_ShouldCallRepositoryWithCorrectCriteria(bool isTest, bool isLive, bool isLicensee)
        {
            // Arrange
            var parameters = new CCSpreadPolicyParamModel
            {
                IsTest = isTest,
                IsLive = isLive,
                IsLicensee = isLicensee,
                RepUpdateTime = DateTime.Now,
                RowNum = 5,
                MaxRecords = 200
            };

            var isWriteLogInfo = false;
            var expectedSpreadPolicy = new List<CCSpreadPolicyEntity>();
            var expectedSpreadPolicyDetail = new List<CCSpreadPolicyDetailEntity>();

            _repository.FetchMultiple<CCSpreadPolicyEntity, CCSpreadPolicyDetailEntity>(Arg.Any<GetCCPeakHourDiffCriteria>())
                .Returns((expectedSpreadPolicy, expectedSpreadPolicyDetail));

            // Act
            var result = _peakHourSpreadDataAccess.GetCCPeakHourDiff(parameters, isWriteLogInfo);

            // Assert
            _repository.Received(1).FetchMultiple<CCSpreadPolicyEntity, CCSpreadPolicyDetailEntity>(
                Arg.Is<GetCCPeakHourDiffCriteria>(c => 
                    c.IsTest == isTest &&
                    c.IsLive == isLive &&
                    c.IsLicensee == isLicensee &&
                    c.RepUpdateTime == parameters.RepUpdateTime &&
                    c.RowNum == parameters.RowNum &&
                    c.MaxRecords == parameters.MaxRecords));
        }

        [Fact]
        public void GetCCPeakHourDiff_WhenParametersAreNull_ShouldHandleGracefully()
        {
            // Arrange
            CCSpreadPolicyParamModel parameters = null;
            var isWriteLogInfo = false;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _peakHourSpreadDataAccess.GetCCPeakHourDiff(parameters, isWriteLogInfo));
        }       

        [Fact]
        public void GetCCPeakHourDiff_WhenRepositoryReturnsNullCollections_ShouldReturnEmptyModel()
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

            var isWriteLogInfo = false;
            List<CCSpreadPolicyEntity> expectedSpreadPolicy = null;
            List<CCSpreadPolicyDetailEntity> expectedSpreadPolicyDetail = null;

            _repository.FetchMultiple<CCSpreadPolicyEntity, CCSpreadPolicyDetailEntity>(Arg.Any<GetCCPeakHourDiffCriteria>())
                .Returns((expectedSpreadPolicy, expectedSpreadPolicyDetail));

            // Act
            var result = _peakHourSpreadDataAccess.GetCCPeakHourDiff(parameters, isWriteLogInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.SpreadPolicy);
            Assert.Null(result.SpreadPolicyDetail);
        }
    }
} 