namespace FCG.Domain.Entities;

/// <summary>
/// Represents the relationship between a user and a game in their library
/// </summary>
public class UserGame : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid GameId { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public decimal PurchasePrice { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;
    public Game Game { get; private set; } = null!;

    private UserGame(Guid userId, Guid gameId, decimal purchasePrice)
    {
        UserId = userId;
        GameId = gameId;
        PurchasePrice = purchasePrice;
        PurchaseDate = DateTime.UtcNow;
    }

    public static UserGame Create(Guid userId, Guid gameId, decimal purchasePrice)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        if (gameId == Guid.Empty)
            throw new ArgumentException("Game ID cannot be empty", nameof(gameId));

        if (purchasePrice < 0)
            throw new ArgumentException("Purchase price cannot be negative", nameof(purchasePrice));

        return new UserGame(userId, gameId, purchasePrice);
    }
}
