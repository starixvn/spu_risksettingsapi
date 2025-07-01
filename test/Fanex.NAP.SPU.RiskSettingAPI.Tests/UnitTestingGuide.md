# Unit Testing Guide

## Overview
This guide covers how to write effective unit tests for the RiskSettingAPI project using xUnit and NSubstitute.

## Test Structure

### 1. Test Class Naming Convention
- Test class name should match the class being tested + "Test"
- Example: `PeakHourSpreadService` → `PeakHourSpreadServiceTest`

### 2. Test Method Naming Convention
Use the pattern: `MethodName_Scenario_ExpectedResult`

Examples:
- `GetCCPeakHourRange_WhenSuccess_ShouldReturnExpectedData`
- `GetCCPeakHourRange_WhenDataAccessThrowsException_ShouldPropagateException`
- `GetCustomerClassPeakHourRange_WhenIsTestFalse_ShouldCallDataAccessWithCorrectParameter`

### 3. Test Structure (AAA Pattern)
```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange - Set up test data and mocks
    var expectedResult = new List<Entity>();
    _mockService.Method().Returns(expectedResult);

    // Act - Execute the method being tested
    var result = _service.Method();

    // Assert - Verify the results
    Assert.NotNull(result);
    Assert.Equal(expectedResult, result);
}
```

## Mocking with NSubstitute

### 1. Setting up Mocks
```csharp
public class ServiceTest
{
    private readonly IService _service;
    private readonly IRepository _repository;

    public ServiceTest()
    {
        _repository = Substitute.For<IRepository>();
        _service = new Service(_repository);
    }
}
```

### 2. Mock Return Values
```csharp
// Return a simple value
_repository.GetById(1).Returns(new Entity { Id = 1 });

// Return a collection
_repository.GetAll().Returns(new List<Entity> { entity1, entity2 });

// Return null
_repository.GetById(999).Returns((Entity)null);

// Return based on parameters
_repository.GetById(Arg.Any<int>()).Returns(new Entity());
```

### 3. Mock Exceptions
```csharp
// Throw exception
_repository.GetById(1).Returns(x => throw new Exception("Database error"));

// Throw specific exception
_repository.GetById(1).Returns(x => throw new InvalidOperationException("Invalid ID"));
```

### 4. Verify Mock Calls
```csharp
// Verify method was called
_repository.Received(1).GetById(1);

// Verify method was not called
_repository.DidNotReceive().GetById(999);

// Verify method was called with any parameter
_repository.Received(1).GetById(Arg.Any<int>());

// Verify method was called with specific parameter
_repository.Received(1).GetById(Arg.Is<int>(x => x > 0));
```

## Test Types

### 1. Unit Tests ([Fact])
Test individual methods in isolation.

```csharp
[Fact]
public void GetCCPeakHourRange_WhenSuccess_ShouldReturnExpectedData()
{
    // Arrange
    var expectedResult = new List<CCPeakHourRangeEntity>();
    _dataAccess.GetCCPeakHourRange(true).Returns(expectedResult);

    // Act
    var result = _service.GetCCPeakHourRange(true);

    // Assert
    Assert.Equal(expectedResult, result);
}
```

### 2. Parameterized Tests ([Theory])
Test the same logic with different inputs.

```csharp
[Theory]
[InlineData(true)]
[InlineData(false)]
public void GetCCPeakHourRange_WithDifferentIsTestValues_ShouldCallDataAccessCorrectly(bool isTest)
{
    // Arrange
    var expectedResult = new List<CCPeakHourRangeEntity>();
    _dataAccess.GetCCPeakHourRange(isTest).Returns(expectedResult);

    // Act
    var result = _service.GetCCPeakHourRange(isTest);

    // Assert
    _dataAccess.Received(1).GetCCPeakHourRange(isTest);
    Assert.Equal(expectedResult, result);
}
```

### 3. Exception Tests
Test that exceptions are properly handled.

```csharp
[Fact]
public void GetCCPeakHourRange_WhenDataAccessThrowsException_ShouldPropagateException()
{
    // Arrange
    var expectedException = new Exception("Database error");
    _dataAccess.GetCCPeakHourRange(true).Returns(x => throw expectedException);

    // Act & Assert
    var exception = Assert.Throws<Exception>(() => _service.GetCCPeakHourRange(true));
    Assert.Equal(expectedException.Message, exception.Message);
}
```

## Testing Different Layers

### 1. Service Layer Tests
- Test business logic
- Mock dependencies (repositories, external services)
- Test both success and failure scenarios

### 2. Controller Tests
- Test HTTP responses
- Mock service dependencies
- Test input validation
- Test async methods

### 3. Repository/Data Access Tests
- Use in-memory databases or test databases
- Test CRUD operations
- Test complex queries

## Best Practices

### 1. Test Independence
- Each test should be independent
- Don't rely on other tests
- Reset mocks between tests if needed

### 2. Test Data
- Use meaningful test data
- Create helper methods for common test data
- Use builders for complex objects

### 3. Assertions
- Be specific in assertions
- Test one thing per test
- Use descriptive assertion messages

### 4. Mock Verification
- Verify that dependencies are called correctly
- Don't over-mock
- Focus on behavior, not implementation

### 5. Test Coverage
- Aim for high test coverage
- Test edge cases
- Test error scenarios
- Test boundary conditions

## Common Patterns

### 1. Test Data Builders
```csharp
public static class TestDataBuilder
{
    public static CCSpreadPolicyParamModel CreateValidParameters()
    {
        return new CCSpreadPolicyParamModel
        {
            IsTest = true,
            IsLive = true,
            IsLicensee = false,
            RepUpdateTime = DateTime.Now,
            RowNum = 1,
            MaxRecords = 100
        };
    }
}
```

### 2. Async Test Helpers
```csharp
[Fact]
public async Task GetDataAsync_WhenSuccess_ShouldReturnData()
{
    // Arrange
    var expectedResult = new List<Entity>();
    _repository.GetDataAsync().Returns(Task.FromResult(expectedResult));

    // Act
    var result = await _service.GetDataAsync();

    // Assert
    Assert.Equal(expectedResult, result);
}
```

### 3. Exception Testing
```csharp
[Fact]
public void Method_WhenExceptionOccurs_ShouldHandleGracefully()
{
    // Arrange
    _repository.GetData().Returns(x => throw new Exception("Error"));

    // Act
    var result = _service.Method();

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
}
```

## Running Tests

### 1. Command Line
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test test/Fanex.NAP.SPU.RiskSettingAPI.Tests/

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### 2. Visual Studio
- Use Test Explorer
- Run individual tests
- Debug tests
- View test results

## Test Configuration

### 1. Test Project File
Ensure your test project includes necessary packages:
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.6.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.4" />
    <PackageReference Include="NSubstitute" Version="5.1.0" />
</ItemGroup>
```

### 2. Test Categories
Use test categories for organization:
```csharp
[Trait("Category", "Unit")]
[Fact]
public void TestMethod() { }

[Trait("Category", "Integration")]
[Fact]
public void IntegrationTest() { }
```

## Troubleshooting

### 1. Common Issues
- Mock not returning expected values
- Async test not completing
- Test data not being reset
- Dependencies not being mocked properly

### 2. Debugging Tests
- Use breakpoints in test methods
- Check mock setup
- Verify test data
- Use test output for debugging

This guide should help you write effective unit tests for the RiskSettingAPI project. Remember to follow the patterns established in the existing test files and maintain consistency across the test suite. 