namespace FCG.Domain.Entities;

public class Game : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Genre { get; private set; }
    public decimal Price { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public string Publisher { get; private set; }
    public bool IsActive { get; private set; }

    private Game(
        string title,
        string description,
        string genre,
        decimal price,
        DateTime releaseDate,
        string publisher)
    {
        Title = title;
        Description = description;
        Genre = genre;
        Price = price;
        ReleaseDate = releaseDate;
        Publisher = publisher;
        IsActive = true;
    }

    public static Game Create(
        string title,
        string description,
        string genre,
        decimal price,
        DateTime releaseDate,
        string publisher)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        return new Game(title, description, genre, price, releaseDate, publisher);
    }

    public void Update(
        string title,
        string description,
        string genre,
        decimal price,
        DateTime releaseDate,
        string publisher)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        Title = title;
        Description = description;
        Genre = genre;
        Price = price;
        ReleaseDate = releaseDate;
        Publisher = publisher;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
