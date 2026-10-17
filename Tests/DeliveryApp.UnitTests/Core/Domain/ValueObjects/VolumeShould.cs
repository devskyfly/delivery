using DeliveryApp.Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.ValueObjects;

public class VolumeShould
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectNonPositiveValues(int value)
    {
        Volume.Create(value).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CreatePositiveValue()
    {
        var result = Volume.Create(3);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(3);
    }

    [Fact]
    public void CompareByVolumeValue()
    {
        Volume.Create(3).Value.Should().Be(Volume.Create(3).Value);
        Volume.Create(3).Value.Should().NotBe(Volume.Create(4).Value);
    }
}
