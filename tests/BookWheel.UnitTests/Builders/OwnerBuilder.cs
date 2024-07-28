using BookWheel.Domain.AggregateRoots;

namespace BookWheel.UnitTests.Builders;

public class OwnerBuilder
{
    private Guid Id { get; set; } = Guid.NewGuid();

    private string Name { get; set; } = Guid.NewGuid().ToString().Substring(0,10);
    private string Surname { get; set; } = Guid.NewGuid().ToString().Substring(0,10);
    private string Email { get; set; } = Guid.NewGuid().ToString().Substring(0,10).Insert(5,"@");
    
    public OwnerBuilder WithName(string name)
    {
        Name = name;
        return this;
    }
    public OwnerBuilder WithSurname(string surname)
    {
        Surname = surname;
        return this;
    }
    public OwnerBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public OwnerBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }


    public OwnerUserRoot Build()
    {
        return new OwnerUserRoot(Id,Name,Surname,Email, "+994518209692");
    }
}