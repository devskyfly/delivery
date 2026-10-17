using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.ValueObjects;

public class MaxVolume: ValueObject
{
    public int Value { get; private set; }
    public static readonly int MAX_VOLUME = 20;
    
    private MaxVolume()
    {
        
    }

    private MaxVolume(int value) : this()
    {
        Value = value;
    }

    public static Result<MaxVolume, Error> Create(int value)
    {
        if (value <= 0)
        {
            return GeneralErrors.ValueMustBeGreaterThan(nameof(value), value, 0);
        }

        if (value > MAX_VOLUME)
        {
            return GeneralErrors.ValueMustBeGreaterThan(nameof(value), value, MAX_VOLUME);
        }
        return new MaxVolume(value);
    }

    public bool CanContain(IEnumerable<Volume> volumes)
    {
        var remaining = Value;
        foreach (var volume in volumes)
        {
            if (volume.Value > remaining)
            {
                return false;
            }

            remaining -= volume.Value;
        }

        return true;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}