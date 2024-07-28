using BookWheel.Domain.AggregateRoots;

namespace BookWheel.UnitTests.Builders;

public class CustomerBuilder
{
    private Guid Id { get; set; } = Guid.NewGuid();

    private string Name { get; set; } = Guid.NewGuid().ToString().Substring(0,10);
    private string Surname { get; set; } = Guid.NewGuid().ToString().Substring(0,10);
    private string Email { get; set; } = Guid.NewGuid().ToString().Substring(0,10).Insert(5,"@");
    
    public CustomerBuilder WithName(string name)
    {
        Name = name;
        return this;
    }
    public CustomerBuilder WithSurname(string surname)
    {
        Surname = surname;
        return this;
    }
    public CustomerBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public CustomerBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }


    public CustomerUserRoot Build()
    {
        return new CustomerUserRoot(Id,Name,Surname,Email, "+994518209692");
    }

}