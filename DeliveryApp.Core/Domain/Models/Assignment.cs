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
        OrderId = orderId;
        Volume = volume;
        Location = location;
        Status = status;
    }

    public static Result<Assignment,Error> Create(Guid orderId, Volume volume, Location location)
    {
        var status = Status.Assigned;
        if (status != Status.Assigned)
        {
            GeneralErrors.ValueIsInvalid(nameof(status), status);
        }
        return new Assignment(orderId, volume, location, status);
    }

    public Result<Assignment, Error> Complete(Location courierLocation)
    {
        int distance = this.Location.DistanceTo(courierLocation);
        if (distance > 1)
        {
            return GeneralErrors.ValueIsInvalid(nameof(distance), distance);
        }
        this.Status = Status.Completed;
        return this;
    }
    
}