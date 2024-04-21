using System.Linq.Expressions;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Specifications;

namespace BookWheel.Application.Specifications.Customer;

public class GetCustomerByIdSpecification : Specification<CustomerUserRoot>
{
    public GetCustomerByIdSpecification(Guid Id) : 
        base(u=>u.Id == Id)
    {
    }
}