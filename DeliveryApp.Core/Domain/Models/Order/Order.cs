using CSharpFunctionalExtensions;
using Ddd;
using DeliveryApp.Core.Domain.ValueObjects;
using Errs;

namespace DeliveryApp.Core.Domain.Models;

public class Order: Aggregate<Guid>
{
    public Location Location { get; private set; }

    public Volume Volume { get; private set; }
    
    public OrderStatus Status { get; private set;}
    
    private Order()
    {
        
    }

    private Order(Guid id, Volume volume, Location location, OrderStatus status)
    {
        Id = id;
        Volume = volume;
        Location = location;
        Status = status;
    }

    public static Order Create(Guid id, Volume volume, Location location)
    {
        var status = OrderStatus.Created;
        return new Order(id, volume, location, status);
    }

    public Result<Order, Error> Assign()
    {
        if (Status != OrderStatus.Created)
        {
            return Errors.CantAssignOrder();
        }

        var status = OrderStatus.Assigned;
        Status = status;
        return this;
    }

    public Result<Order,Error> Complete()
    {
        if (Status != OrderStatus.Assigned)
        {
            return Errors.CantCompleteOrder();
        }
        Status = OrderStatus.Completed;
        return this;
    }
    public static class Errors
    {
        public static Error CantAssignOrder()
        {
            return new Error(nameof(Status).ToUpperInvariant(), $"Статус заказа не может быть переведен в статус '{OrderStatus.Assigned}'.");
        }
    
        public static Error CantCompleteOrder()
        {
            return new Error(nameof(Status).ToUpperInvariant(), $"Статус заказа не может быть переведен в статус '{OrderStatus.Completed}'.");
        }
    } 
}