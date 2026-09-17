using CSharpFunctionalExtensions;
namespace DeliveryApp.Core.Domain.ValueObjects;

public class Location: ValueObject
{
    public const int MAX_X_VALUE = 10;
    public const int MAX_Y_VALUE = 10;
    
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
    
    public static int CountDistance(Location p1, Location p2)
    {
        return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}