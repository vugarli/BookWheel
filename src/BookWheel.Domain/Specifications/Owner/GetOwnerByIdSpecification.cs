using BookWheel.Domain.AggregateRoots;

namespace BookWheel.Domain.Specifications.Owner;

public class GetOwnerByIdSpecification : Specification<OwnerUserRoot>
{
    public GetOwnerByIdSpecification(Guid Id) 
        : base(c=>c.Id == Id)
    {
        //AddInclude(o=>o.Location);
    }
}