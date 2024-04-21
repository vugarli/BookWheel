using BookWheel.Domain.AggregateRoots;

namespace BookWheel.UnitTests.Builders;

public static class CustomerProvider
{

    public static CustomerUserRoot GetCustomer(Guid customerId,string name,string email)
    {
        var user = new CustomerUserRoot(customerId,name,name,email);

        return user;
    }
    
}