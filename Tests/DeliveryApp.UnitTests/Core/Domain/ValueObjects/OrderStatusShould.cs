using DeliveryApp.Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.ValueObjects;

public class OrderStatusShould
{
    [Fact]
    public void ExposeTheThreeSupportedStatuses()
    {
        OrderStatus.Created.Name.Should().Be("CREATED");
        OrderStatus.Assigned.Name.Should().Be("ASSIGNED");
        OrderStatus.Completed.Name.Should().Be("COMPLETED");
    }

    [Fact]
    public void CompareStatusesByName()
    {
        OrderStatus.Created.Should().Be(OrderStatus.Created);
        OrderStatus.Created.Should().NotBe(OrderStatus.Assigned);
        OrderStatus.Assigned.Should().NotBe(OrderStatus.Completed);
    }
}
