using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.Models;
using DeliveryApp.Core.Domain.ValueObjects;
using Errs;

public class Assignment: Entity<Guid>
{
    public Guid OrderId
    {
        get;
        private set;
    }
    
    public Location Location {
        get;
        private set;
    }

    public Volume Volume
    {
        get;
        private set;
    }

    public Status Status
    {
        get;
        private set;
    }
    
    private Assignment()
    {
        
    }
    
    private Assignment(Guid orderId, Volume volume, Location location, Status status): this()
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Volume = volume;
        Location = location;
        Status = status;
    }

    public static Result<Assignment,Error> Create(Guid orderId, Volume volume, Location location)
    {
        var status = Status.Assigned;
        return new Assignment(orderId, volume, location, status);
    }

    public Result<Assignment, Error> Complete(Location courierLocation)
    {
        int distance = this.Location.DistanceTo(courierLocation);
        if (distance > Location.MIN_LOCATION_DISTANCE_FOR_COMPLETE)
        {
            return Errors.CantCompleteAssignment();
        }
        this.Status = Status.Completed;
        return this;
    }
    
    public static class Errors
    {
        public static Error CantCompleteAssignment()
        {
            return new Error(nameof(Location),
                $"Невозможно завершить назначение, потому что дистанция между курьером и заказом больше {Location.MIN_LOCATION_DISTANCE_FOR_COMPLETE}");
        }
    }
}