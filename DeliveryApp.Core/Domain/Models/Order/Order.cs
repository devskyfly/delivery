using Ddd;

namespace DeliveryApp.Core.Domain.Models;

public class Order: Aggregate<Guid>
{
    private Order()
    {
        
    }

    //private Order(): this()
    //{
    //}

    public Order Create()
    {
        return new Order();
    }
}