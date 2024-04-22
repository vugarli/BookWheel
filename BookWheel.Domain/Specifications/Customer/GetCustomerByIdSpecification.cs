using BookWheel.Domain.AggregateRoots;

namespace BookWheel.Domain.Specifications.Customer;

public class GetCustomerByIdSpecification : Specification<CustomerUserRoot>
{
    public GetCustomerByIdSpecification(Guid Id) : 
        base(u=>u.Id == Id)
    {
    }
}