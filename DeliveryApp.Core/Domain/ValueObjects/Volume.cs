using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.ValueObjects;

public class Volume: ValueObject
{
    public int Value { get; private set; }

    private Volume()
    {
        
    }

    public Volume(int value) : this()
    {
        Value = value;
    }

    public static Result<Volume, Error> Create(int value)
    {
        if (value <= 0)
        {
            return GeneralErrors.ValueMustBeGreaterThan(nameof(value), value, 0);
        }
        return new Volume(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}