using CSharpFunctionalExtensions;
using Ddd;
using DeliveryApp.Core.Domain.ValueObjects;
using Errs;

namespace DeliveryApp.Core.Domain.Models;

public class Courier: Aggregate<Guid>
{
    public const int MAX_MOVEMENT_DISTANCE_STEP = 1;
    
    public string Name {
        get;
        private set;
    }
    public Location Location {
        get;
        private set;
    }
    
    public MaxVolume MaxVolume { get; private set;}
    
    public List<Assignment> Assignments { get; private set;}
    
    private Courier()
    {
        
    }
    
    private Courier(string name, Location location, MaxVolume maxVolume): this()
    {
        Id =  Guid.NewGuid();
        Name = name;
        Location = location;
        Assignments = new List<Assignment>();
        MaxVolume = maxVolume;
    }
    
    public static Result<Courier, Error> Create(string name, Location location)
    {
        var maxVolume = MaxVolume.Create(MaxVolume.MAX_VOLUME);
        if (maxVolume.IsFailure) return maxVolume.Error;
        return new Courier(name, location, maxVolume.Value);
    }

    public Result<Courier, Error> AssignOrder(Order order)
    {
        if (!CanAssignOrder(order))
        {
            return Errors.CantAssignOrder();
        }

        var assignmentResult = Assignment.Create(order.Id, order.Volume, order.Location);

        if (assignmentResult.IsFailure)
        {
            return assignmentResult.Error;
        }

        Assignments.Add(assignmentResult.Value);
        return this;
    }

    public Result<Courier, Error> Move(Location location)
    {
        if (this.Location.DistanceTo(location) > MAX_MOVEMENT_DISTANCE_STEP)
        {
            return Errors.CantMove();
        }

        Location = location;

        return this;
    }

    public Result<Courier, Error> CompleteAssignment(Order order)
    {
        if (!CanCompleteOrder(order))
        {
            return Errors.CantCompleteOrder();
        }
        var assignment = Assignments.Find(x => x.OrderId == order.Id);
        if (assignment is null)
        {
            return Errors.CantCompleteOrder();
        }

        var assignmentResult = assignment.Complete(this.Location);
        if (assignmentResult.IsFailure) return assignmentResult.Error;
        return this;
    }
    
    public bool CanCompleteOrder(Order order)
    {
        return Location.DistanceTo(order.Location) <= Location.MIN_LOCATION_DISTANCE_FOR_COMPLETE;
    }
    
    public bool CanAssignOrder(Order order)
    {
        return MaxVolume.CanContain(Assignments
            .Select(assignment => assignment.Volume)
            .Append(order.Volume));
    }
    public static class Errors
    {
        public static Error CantAssignOrder()
        {
            return new Error(nameof(Assignments),
                $"Невозможно назначить заказ, сумма объемов {nameof(Assignments)} должна быть больше 0 и меньше или равно ${ValueObjects.MaxVolume.MAX_VOLUME}.");
        }
        
        public static Error CantCompleteOrder()
        {
            return new Error(nameof(Assignments),
                $"Невозможно завершить заказ, потому что дистанция между курьером и токой заказа больше {Location.MIN_LOCATION_DISTANCE_FOR_COMPLETE}.");
        }
        
        public static Error CantMove()
        {
            return new Error(nameof(Location),
                $"Невозможно переместить курьера, шаг изменения дистанции больше {MAX_MOVEMENT_DISTANCE_STEP}.");
        }
    }
}
