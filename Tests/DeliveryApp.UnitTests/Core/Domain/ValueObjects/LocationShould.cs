namespace DeliveryApp.UnitTests.Core.Domain.ValueObjects;
using System;
using DeliveryApp.Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

public class LocationShould
{
    [Fact]
    public void BeCorrectWhenParamsAreCorrectOnCreated()
    {
        var location = Location.Create(1, 10);
        location.X.Should().Be(1);
        location.Y.Should().Be(10);
    }
    
    [Fact]
    public void CountDistanceCLocationsCorrectly()
    {
        var location1 = Location.Create(2, 6);
        var location2 = Location.Create(4, 9);
        var distance = location1.DistanceTo(location2);
        distance.Should().Be(5);
    }
    
    [Theory]
    [InlineData(0,1)]
    [InlineData(1,0)]
    [InlineData(11,1)]
    [InlineData(1,11)]
    public void ThrowExceptionOnCreate(int x, int y)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var location = Location.Create(x, y);
        });
    }

    [Fact]
    public void BeEqualWhenCoordinatesAreEqual()
    {
        Location.Create(4, 7).Should().Be(Location.Create(4, 7));
    }

    [Fact]
    public void BeDifferentWhenCoordinatesDiffer()
    {
        Location.Create(4, 7).Should().NotBe(Location.Create(4, 8));
    }

    [Fact]
    public void CreateRandomLocationWithinBounds()
    {
        var location = Location.CreateRandom();

        location.X.Should().BeInRange(Location.MIN_X_VALUE, Location.MAX_X_VALUE);
        location.Y.Should().BeInRange(Location.MIN_Y_VALUE, Location.MAX_Y_VALUE);
    }
}