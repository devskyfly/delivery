using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.ValueObjects;

public class OrderStatus: ValueObject
{
    public static readonly OrderStatus Created = new (nameof(Created).ToUpperInvariant());
    public static readonly OrderStatus Assigned = new (nameof(Assigned).ToUpperInvariant());
    public static readonly OrderStatus Comleted = new (nameof(Comleted).ToUpperInvariant());
    public string Name {get; private set;}
        
    private OrderStatus()
    {
    }
    
    private OrderStatus(string name)
    {
        Name = name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}