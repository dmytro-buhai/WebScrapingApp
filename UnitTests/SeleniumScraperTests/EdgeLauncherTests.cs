using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using SeleniumScraper;
using OpenQA.Selenium.Edge;

namespace SeleniumScraperTests;

public class EdgeLauncherTests : IDisposable
{
    private Mock<ILogger<EdgeLauncher>> _mockLogger;
    private EdgeLauncher _edgeLauncher;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<EdgeLauncher>>();
        _edgeLauncher = new EdgeLauncher(_mockLogger.Object);
    }

    [Test]
    public void Constructor_ShouldSetProperties()
    {
        // Assert
        _edgeLauncher.EdgeDriverService.Should().NotBeNull();
        _edgeLauncher.EdgeOptions.Should().NotBeNull();
    }

    [Test]
    public void GetEdgeDriver_ShouldReturnDriver()
    {
        // Arrange
        var expectedDriver = new EdgeDriver(_edgeLauncher.EdgeDriverService, _edgeLauncher.EdgeOptions);

        // Act
        _edgeLauncher.StartLauncher();
        var actualDriver = _edgeLauncher.GetEdgeDriver();

        // Assert
        actualDriver.Capabilities.ToString().Should().BeEquivalentTo(expectedDriver.Capabilities.ToString());
        expectedDriver.Quit();
        _edgeLauncher.StopLauncher();
    }

    [Test]
    public void StartLauncher_ShouldStartDriver()
    {
        _edgeLauncher.StartLauncher();

        _edgeLauncher.GetEdgeDriver().Should().NotBeNull();

        _edgeLauncher.StopLauncher();
    }

    [Test]
    public void StopLauncher_ShouldStopDriver()
    {
        // Arrange
        _edgeLauncher.StartLauncher();

        _edgeLauncher.StopLauncher();

        _edgeLauncher.GetEdgeDriver().SessionId.Should().BeNull();
    }

    [Test]
    public void Dispose_ShouldStopDriver()
    {
        // Arrange
        _edgeLauncher.StartLauncher();

        // Act
        _edgeLauncher.Dispose();

        // Assert
        _edgeLauncher.GetEdgeDriver().SessionId.Should().BeNull();
    }

    public void Dispose()
    {
        _edgeLauncher.Dispose();
    }
}
