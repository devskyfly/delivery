using System;
using DeliveryApp.Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.ValueObjects;

public class MaxVolumeShould
{
    [Fact]
    public void AllowVolumesWithinCapacity()
    {
        var maxVolume = MaxVolume.Create(10).Value;
        var volumes = new[]
        {
            Volume.Create(3).Value,
            Volume.Create(7).Value
        };

        maxVolume.CanContain(volumes).Should().BeTrue();
    }

    [Fact]
    public void RejectVolumesExceedingCapacity()
    {
        var maxVolume = MaxVolume.Create(10).Value;
        var volumes = new[]
        {
            Volume.Create(3).Value,
            Volume.Create(8).Value
        };

        maxVolume.CanContain(volumes).Should().BeFalse();
    }

    [Fact]
    public void RejectVolumesExceedingCapacityWithoutIntegerOverflow()
    {
        var maxVolume = MaxVolume.Create(10).Value;
        var volumes = new[]
        {
            Volume.Create(int.MaxValue).Value,
            Volume.Create(1).Value
        };

        maxVolume.CanContain(volumes).Should().BeFalse();
    }

    [Fact]
    public void RejectValuesOutsideAllowedRange()
    {
        MaxVolume.Create(0).IsFailure.Should().BeTrue();
        MaxVolume.Create(-1).IsFailure.Should().BeTrue();
        MaxVolume.Create(MaxVolume.MAX_VOLUME + 1).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void AllowValuesWithinAllowedRange()
    {
        MaxVolume.Create(1).IsSuccess.Should().BeTrue();
        MaxVolume.Create(MaxVolume.MAX_VOLUME).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void AllowAnEmptySetOfVolumes()
    {
        MaxVolume.Create(10).Value.CanContain(Array.Empty<Volume>()).Should().BeTrue();
    }
}
