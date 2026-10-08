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
        if (distance > 1)
        {
            return Errors.CantCompleteAssignment();
        }
        this.Status = Status.Completed;
        return this;
    }
    
    public static int SumVolume(IEnumerable<Assignment> assignments)
    {
        return assignments.Sum(v => v.Volume.Value);
    }
    
    public static class Errors
    {
        // public static Error CantAssignOrder()
        // {
        //     return new Error(nameof(Assignments),
        //         $"Невозможно назначить заказ, сумма объемов {nameof(Assignments)} будет больше ${ValueObjects.MaxVolume.MAX_VOLUME}");
        // }
        
        public static Error CantCompleteAssignment()
        {
            return new Error(nameof(Location),
                $"Невозможно завершить назначение, потому что дистанция между курьером и заказом больше {Location.MIN_LOCATION_DISTANCE_FOR_COMPLETE}");
        }
    }
}