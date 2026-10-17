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

    [Theory]
    [InlineData(5, 8)]
    [InlineData(6, 8)]
    public void CompleteWhenCourierIsAtOrOneStepFromAssignment(int courierX, int courierY)
    {
        var assignmentLocation = Location.Create(5, 8);
        var assignment = Assignment.Create(
            Guid.NewGuid(),
            Volume.Create(3).Value,
            assignmentLocation).Value;

        var result = assignment.Complete(Location.Create(courierX, courierY));

        result.IsSuccess.Should().BeTrue();
        assignment.Status.Should().Be(Status.Completed);
    }

    [Fact]
    public void RejectCompletionWhenCourierIsMoreThanOneStepFromAssignment()
    {
        var assignment = Assignment.Create(
            Guid.NewGuid(),
            Volume.Create(3).Value,
            Location.Create(5, 8)).Value;

        var result = assignment.Complete(Location.Create(7, 8));

        result.IsFailure.Should().BeTrue();
        assignment.Status.Should().Be(Status.Assigned);
    }
}