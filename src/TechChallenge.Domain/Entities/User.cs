namespace TechChallenge.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }

    private User(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public static User Create(string name, string email)
        => new(name, email);
}
