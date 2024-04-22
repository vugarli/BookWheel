using BookWheel.Domain.AggregateRoots;

namespace BookWheel.UnitTests.Builders;

public static class OwnerProvider
{
    public static OwnerUserRoot GetOwner(Guid ownerId,string name,string email)
    {
        var user = new OwnerUserRoot(ownerId,name,name,email);

        return user;
    }
}