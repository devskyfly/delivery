using System;
using System.Linq;
using DeliveryApp.Core.Domain.Models;
using DeliveryApp.Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.Models;

public class CourierShould
{
    [Fact]
    public void BeCreatedWithMaximumCapacityAndNoAssignments()
    {
        var location = Location.Create(5, 8);

        var result = Courier.Create("Courier", location);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Courier");
        result.Value.Location.Should().Be(location);
        result.Value.MaxVolume.Should().Be(MaxVolume.Create(MaxVolume.MAX_VOLUME).Value);
        result.Value.Assignments.Should().BeEmpty();
    }

    [Fact]
    public void AllowAssigningOrdersUpToMaximumCapacity()
    {
        var courier = CreateCourier();
        var firstOrder = CreateOrder(11);
        var secondOrder = CreateOrder(9);

        courier.AssignOrder(firstOrder).IsSuccess.Should().BeTrue();
        var result = courier.AssignOrder(secondOrder);

        result.IsSuccess.Should().BeTrue();
        courier.Assignments.Should().HaveCount(2);
        courier.Assignments.Select(assignment => assignment.OrderId)
            .Should().Equal(firstOrder.Id, secondOrder.Id);
    }

    [Fact]
    public void DetermineWhetherAnotherOrderFitsRemainingCapacity()
    {
        var courier = CreateCourier();
        courier.AssignOrder(CreateOrder(15)).IsSuccess.Should().BeTrue();

        courier.CanAssignOrder(CreateOrder(5)).Should().BeTrue();
        courier.CanAssignOrder(CreateOrder(6)).Should().BeFalse();
    }

    [Fact]
    public void RejectAssignmentThatExceedsMaximumCapacityWithoutChangingAssignments()
    {
        var courier = CreateCourier();
        courier.AssignOrder(CreateOrder(15)).IsSuccess.Should().BeTrue();

        var result = courier.AssignOrder(CreateOrder(6));

        result.IsFailure.Should().BeTrue();
        courier.Assignments.Should().ContainSingle();
        courier.Assignments[0].Volume.Should().Be(Volume.Create(15).Value);
    }

    [Fact]
    public void MoveByAtMostOneStep()
    {
        var courier = CreateCourier(Location.Create(5, 8));

        var result = courier.Move(Location.Create(6, 8));

        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(Location.Create(6, 8));
    }

    [Fact]
    public void RejectMoveByMoreThanOneStepWithoutChangingLocation()
    {
        var courier = CreateCourier(Location.Create(5, 8));

        var result = courier.Move(Location.Create(7, 8));

        result.IsFailure.Should().BeTrue();
        courier.Location.Should().Be(Location.Create(5, 8));
    }

    [Fact]
    public void CompleteAssignedOrderWhenCourierIsAtOrOneStepFromIt()
    {
        var courier = CreateCourier(Location.Create(5, 8));
        var order = CreateOrder(3, Location.Create(6, 8));
        courier.AssignOrder(order).IsSuccess.Should().BeTrue();

        var result = courier.CompleteAssignment(order);

        result.IsSuccess.Should().BeTrue();
        courier.Assignments[0].Status.Should().Be(Status.Completed);
    }

    [Fact]
    public void RejectCompletingAssignedOrderWhenCourierIsTooFarAway()
    {
        var courier = CreateCourier(Location.Create(5, 8));
        var order = CreateOrder(3, Location.Create(7, 8));
        courier.AssignOrder(order).IsSuccess.Should().BeTrue();

        var result = courier.CompleteAssignment(order);

        result.IsFailure.Should().BeTrue();
        courier.Assignments[0].Status.Should().Be(Status.Assigned);
    }

    [Fact]
    public void RejectCompletingOrderThatWasNotAssignedToCourier()
    {
        var courier = CreateCourier();
        var order = CreateOrder(3);

        var result = courier.CompleteAssignment(order);

        result.IsFailure.Should().BeTrue();
        courier.Assignments.Should().BeEmpty();
    }

    private static Courier CreateCourier(Location? location = null)
    {
        return Courier.Create("Courier", location ?? Location.Create(5, 8)).Value;
    }

    private static Order CreateOrder(int volume, Location? location = null)
    {
        return Order.Create(
            Guid.NewGuid(),
            Volume.Create(volume).Value,
            location ?? Location.Create(5, 8));
    }
}
