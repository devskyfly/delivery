using System.Numerics;
using System.Runtime.InteropServices;
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
    }
    public Location Location {
        get;
    }
    
    public MaxVolume MaxVolume { get; private set;}
    
    public List<Assignment> Assignments { get; private set;}
    
    private Courier()
    {
        
    }
    
    private Courier(string name, Location location): this()
    {
        Name = name;
        Location = location;
    }
    
    public static Courier Create(string name, Location location)
    {
        return new Courier(name, location);
    }

    public Result<Courier, Error> AssignOrder(Order order)
    {
        if (!CanAssignOrder(order))
        {
            
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
        if (this.Location.DistanceTo(location) >= MAX_MOVEMENT_DISTANCE_STEP)
        {
            return Errors.CantMove();
        }

        return this;
    }

    public Result<Courier, Error> CompleteAssignment(Order order)
    {
        if (!CanAssignOrder(order))
        {
            return Errors.CantAssignOrder();
        }
        var assignment = Assignments.Find(x=>x.OrderId==order.Id);
        var assignmentResult = assignment.Complete(this.Location);
        if (assignmentResult.IsFailure) return assignmentResult.Error;
        return this;
    }
    
    public bool CanCompleteOrder(Order order)
    {
        var distanse = Location.DistanceTo(order.Location);
        if (distanse <= Location.MIN_LOCATION_DISTANCE_FOR_COMPLETE)
        {
            return true; 
        }
        return false;
    }
    
    public bool CanAssignOrder(Order order)
    {
        var volumeSum=Assignment.SumVolume(Assignments);
        if (volumeSum + order.Volume.Value > MaxVolume.Value)
        {
            return false;
        }
        return true;
    }
    public static class Errors
    {
        public static Error CantAssignOrder()
        {
            return new Error(nameof(Assignments),
                $"Невозможно назначить заказ, сумма объемов {nameof(Assignments)} будет больше ${ValueObjects.MaxVolume.MAX_VOLUME}.");
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

