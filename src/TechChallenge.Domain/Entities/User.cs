using TechChallenge.Domain.Enumerations;

namespace TechChallenge.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Role Role { get; private set; }
    public string? CognitoUserId { get; private set; }

    private User(string name, string email, Role role)
    {
        Name = name;
        Email = email;
        Role = role;
    }

    public static User CreateUser(string name, string email)
        => new(name, email, Role.User);

    public static User CreateAdmin(string name, string email)
        => new(name, email, Role.Admin);

    public void SetCognitoUserId(string cognitoUserId)
        => CognitoUserId = cognitoUserId;
}
