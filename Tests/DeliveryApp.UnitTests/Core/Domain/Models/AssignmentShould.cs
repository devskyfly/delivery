using System;
using DeliveryApp.Core.Domain.Models;
using DeliveryApp.Core.Domain.ValueObjects;

namespace DeliveryApp.UnitTests.Core.Domain.Models;
using FluentAssertions;
using Xunit;

public class AssignmentShould
{
    [Fact]
    public void BeCorrectOnCreated()
    {
        var guid = new Guid();
        var volume = Volume.Create(3);
        var location = Location.Create(5, 8);
        var assignment = Assignment.Create(guid, volume.Value, location);
        assignment.Value.Status.Should().Be(Status.Assigned);
        assignment.Value.Volume.Should().Be(volume.Value);
        assignment.Value.Location.Should().Be(location);
    }
}