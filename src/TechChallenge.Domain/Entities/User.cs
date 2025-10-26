using TechChallenge.Domain.Common;

namespace TechChallenge.Domain.Entities;

public class User : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }

    private User(string name, string email)
    {
        Name = name;
        Email = email;
    }
}