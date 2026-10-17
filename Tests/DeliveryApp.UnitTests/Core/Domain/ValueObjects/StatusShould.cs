using DeliveryApp.Core.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.ValueObjects;

public class StatusShould
{
    [Fact]
    public void ExposeAssignedAndCompletedStates()
    {
        Status.Assigned.Name.Should().Be("assigned");
        Status.Completed.Name.Should().Be("completed");
    }

    [Fact]
    public void CompareStatusesByName()
    {
        Status.Assigned.Should().Be(Status.Assigned);
        Status.Assigned.Should().NotBe(Status.Completed);
    }
}
