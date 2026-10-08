using CSharpFunctionalExtensions;
namespace DeliveryApp.Core.Domain.ValueObjects;

public class Location: ValueObject
{
    public const int MIN_LOCATION_DISTANCE_FOR_COMPLETE = 100;
    public const int MAX_X_VALUE = 10;
    public const int MIN_X_VALUE = 1;
    public const int MAX_Y_VALUE = 10;
    public const int MIN_Y_VALUE = 1;
    
    public int X { get; }
    public int Y { get; }
    
    private Location()
    {
    }
    
    private Location(int x, int y) : this()
    {
        X = x;
        Y = y;
    }

    public static Location Create(int x, int y)
    {
        if (x > MAX_X_VALUE)
        {
            throw new ArgumentException($"Argument {nameof(x)} is gt {MAX_X_VALUE}");
        }
        if ( x <= 0)
        {
            throw new ArgumentException($"Argument{nameof(x)} is lt 0");
        }
        if (y > MAX_Y_VALUE)
        {
            throw new ArgumentException($"Argument {nameof(y)} is gt {MAX_Y_VALUE}");
        }
        if ( y <= 0)
        {
            throw new ArgumentException($"Argument{nameof(y)} is lt 0");
        }

        return new Location(x, y);
    }

    public static Location CreateRandom()
    {
        int x = Random.Shared.Next(MIN_X_VALUE, MAX_X_VALUE + 1);
        int y = Random.Shared.Next(MIN_Y_VALUE, MAX_Y_VALUE + 1);
        return Create(x, y);
    }
    
    public int DistanceTo(Location p)
    {
        return Math.Abs(X - p.X) + Math.Abs(Y - p.Y);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}