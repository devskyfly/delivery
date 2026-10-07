using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.ValueObjects;

public class Status : ValueObject
{
    public static Status Assigned = new(nameof(Assigned).ToLowerInvariant());
    public static Status Completed = new(nameof(Completed).ToLowerInvariant());

    public string Name {get; private set;}
    
    private Status()
    {
        
    }

    private Status(string name) : this()
    {
        Name = name;
    }

    public Status Create(string name)
    {
        return new Status(name);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}