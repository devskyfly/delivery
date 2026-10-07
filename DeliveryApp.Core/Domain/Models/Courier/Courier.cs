using System.Runtime.InteropServices;
using Ddd;
using DeliveryApp.Core.Domain.ValueObjects;

namespace DeliveryApp.Core.Domain.Models;

public class Courier: Aggregate<Guid>
{
    public string Name {
        get;
    }
    public Location Location {
        get;
    }
    
    private Courier()
    {
        
    }
    
    private Courier(string name, Location location): this()
    {
        Name = name;
        Location = location;
    }

    public static Courier Create(string name)
    {
        var location = ValueObjects.Location.CreateRandom();
        return new Courier(name, location);
    }
}