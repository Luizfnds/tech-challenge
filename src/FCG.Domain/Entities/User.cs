using FCG.Domain.Enumerations;

namespace FCG.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Role Role { get; private set; }
    public string? AccountId { get; private set; }

    // Navigation property
    private readonly List<UserGame> _userGames = new();
    public IReadOnlyCollection<UserGame> UserGames => _userGames.AsReadOnly();

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

    public void SetAccountId(string accountId)
        => AccountId = accountId;
}
