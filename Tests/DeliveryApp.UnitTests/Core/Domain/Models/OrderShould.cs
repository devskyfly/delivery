using System;
using DeliveryApp.Core.Domain.Models;
using DeliveryApp.Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.Models;

public class OrderShould
{
    [Fact]
    public void BeCreatedWithCreatedStatus()
    {
        var id = Guid.NewGuid();
        var location = Location.Create(5, 8);
        var volume = Volume.Create(3).Value;

        var order = Order.Create(id, volume, location);

        order.Id.Should().Be(id);
        order.Location.Should().Be(location);
        order.Volume.Should().Be(volume);
        order.Status.Should().Be(OrderStatus.Created);
    }

    [Fact]
    public void BecomeAssignedWhenCreatedOrderIsAssigned()
    {
        var order = CreateOrder();

        var result = order.Assign();

        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Assigned);
    }

    [Fact]
    public void RejectAssignmentWhenOrderIsNotCreated()
    {
        var order = CreateOrder();
        order.Assign().IsSuccess.Should().BeTrue();

        var result = order.Assign();

        result.IsFailure.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Assigned);
    }

    [Fact]
    public void RejectCompletionWhenOrderIsNotAssigned()
    {
        var order = CreateOrder();

        var result = order.Complete();

        result.IsFailure.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Created);
    }

    [Fact]
    public void BecomeCompletedWhenAssignedOrderIsCompleted()
    {
        var order = CreateOrder();
        order.Assign().IsSuccess.Should().BeTrue();

        var result = order.Complete();

        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void RejectCompletionWhenOrderIsAlreadyCompleted()
    {
        var order = CreateOrder();
        order.Assign().IsSuccess.Should().BeTrue();
        order.Complete().IsSuccess.Should().BeTrue();

        var result = order.Complete();

        result.IsFailure.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Completed);
    }

    private static Order CreateOrder()
    {
        return Order.Create(
            Guid.NewGuid(),
            Volume.Create(3).Value,
            Location.Create(5, 8));
    }
}
